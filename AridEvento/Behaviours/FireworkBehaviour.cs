using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmResolver.Collections;
using EventoMX.Behaviours;
using Newtonsoft.Json.Linq;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

namespace EventoMX.Behaviors
{
    public class FireworkBehaviour : MonoBehaviour
    {
        public Player Player { get; private set; }
        public float FlightTime => Time.realtimeSinceStartup - Launched;
        public bool IsLaunched { get; private set; }
        public float Fuse { get; set; } = 2f;
        private float Launched { get; set; } = 0f;

        public ushort TrailEffectID { get; set; } = 139;
        public float TrailRate { get; set; } = 0.05f;

        public ushort ExplosionEffectID { get; set; } = 20;
        public ushort ExplosionDamage { get; set; } = 200;
        public ushort ExplosionRadius { get; set; } = 20;
        public ushort ExplosionEffect { get; set; } = 20;
        public ushort ExplosionEffectCount { get; set; } = 70;
        private readonly System.Random m_Random = new System.Random();
        //public List<ushort> ExplosionEffects { get; } = new List<ushort>() { 124, 130, 312, 134, 139, 124, 130, 134};
        public List<List<ushort>> ExplosionEffects { get; } = new List<List<ushort>>()
        {
            new List<ushort> { 124, 139, 125 },
            new List<ushort> { 130, 131, 139 },
            new List<ushort>() { 124, 130, 312, 134, 139, 124, 130, 134},
            new List<ushort> { 132, 133, 139 },
            new List<ushort> { 134, 135, 139 }
        };

        public EffectTrailer Trailer { get; private set; }
        private GameObject fireworkObject;
        private float m_PrevGravity = 1f;
        public void Awake()
        {
            Player = GetComponent<Player>();
            Trailer = gameObject.AddComponent<EffectTrailer>();
            Trailer.Radius = EffectManager.INSANE;
        }
        private List<ushort> GetRandomFirework()
        {
            if (ExplosionEffects.Count == 0)
            {
                return null;
            }
            int randomIndex = m_Random.Next(ExplosionEffects.Count);
            return ExplosionEffects[randomIndex];
        }
        public void Launch()
        {
            // Create a new GameObject for the firework at the player's position
            fireworkObject = new GameObject("Firework");
            fireworkObject.transform.position = transform.position;

            // Attach the EffectTrailer script to the fireworkObject
            var trailer = fireworkObject.AddComponent<EffectTrailer>();
            trailer.EffectID = TrailEffectID;
            trailer.Rate = TrailRate;
            trailer.Radius = EffectManager.INSANE;

            // Enable trail effects for the fireworkObject
            var effectTrailer = fireworkObject.GetComponent<EffectTrailer>();
            effectTrailer.enabled = true;

            // Set launched
            Launched = Time.realtimeSinceStartup;
            IsLaunched = true;
        }

        public void Abort()
        {
            if (IsLaunched)
            {
                Stop();
                Destroy(Trailer);
                Destroy(this);
            }
        }

        public void FixedUpdate()
        {
            if (IsLaunched)
            {
                fireworkObject.transform.position = new Vector3(fireworkObject.transform.position.x, fireworkObject.transform.position.y + 0.5f, fireworkObject.transform.position.z);
                if (FlightTime >= Fuse)
                {
                    EffectManager.sendEffect(ExplosionEffectID, EffectManager.INSANE, fireworkObject.transform.position);
                    EffectManager.sendEffect(123, EffectManager.INSANE, fireworkObject.transform.position);
                    StartCoroutine(DoExplosionEffects());
                    Stop();
                }
            }
        }

        private void Stop()
        {
            // Destroy the fireworkObject instead of modifying player's position
            Destroy(fireworkObject);
            IsLaunched = false;
            Trailer.enabled = false;
        }

        private IEnumerator DoExplosionEffects(float timeToComplete = 0.2f, bool destroyPost = true)
        {
            if (ExplosionEffects.Count == 0)
            {
                yield break;
            }

            var effectDelay = timeToComplete / ExplosionEffectCount;
            var frameTime = 1f / 50f;
            var effectsPerFrame = 1;
            var initialDelay = effectDelay;

            // If the delay is too short, it will be rounded to the next frame.
            // So run more effects per frame and increase the effect delay instead.
            while (effectDelay < frameTime)
            {
                effectDelay += initialDelay;
                effectsPerFrame += 1;
            }

            var effects = new List<(Vector3 pos, ushort effect)>();
            int firework = UnityEngine.Random.Range(0, ExplosionEffects.Count);

            for (int i = 0; i < ExplosionEffectCount; i++)
            {
                var randomEffect = ExplosionEffects[firework][UnityEngine.Random.Range(0, ExplosionEffects[firework].Count)];

                var position = fireworkObject.transform.position + (UnityEngine.Random.insideUnitSphere * ExplosionRadius);

                effects.Add((position, randomEffect));
            }

            // Order effects inside to out of the sphere to simulate an outward explosion
            var ordered = effects.OrderBy(x => (fireworkObject.transform.position - x.pos).sqrMagnitude);

            var queue = new Queue<(Vector3 pos, ushort effect)>();
            foreach (var order in ordered)
            {
                queue.Enqueue(order);
            }

            while (queue.Count > 0)
            {
                for (int i = 0; i < effectsPerFrame; i++)
                {
                    if (effects.Count == 0)
                        break;
                    var effect = queue.Dequeue();
                    EffectManager.sendEffect(effect.effect, EffectManager.INSANE, effect.pos);
                }
                yield return new WaitForSeconds(effectDelay);
            }

            // If set, destroy the object once the coroutine is finished
            if (destroyPost)
            {
                Destroy(Trailer);
                Destroy(this);
            }
        }
    }
}