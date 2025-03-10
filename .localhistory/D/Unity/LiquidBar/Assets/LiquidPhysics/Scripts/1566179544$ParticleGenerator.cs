using UnityEngine;
using System.Collections;

public class ParticleGenerator : MonoBehaviour
{
    float SPAWN_INTERVAL = 0.025f; // How much time until the next particle spawns
    float lastSpawnTime = float.MinValue; //The last spawn time
    public int PARTICLE_LIFETIME = 3000000; //How much time will each particle live
    public Vector3 particleForce; //Is there a initial force particles should have?
    public DynamicParticle.STATES particlesState = DynamicParticle.STATES.WATER; // The state of the particles spawned
    public Transform particlesParent; // Where will the spawned particles will be parented (To avoid covering the whole inspector with them)
    public bool emit = false;
    public Color color;
    void Start() { }

    void Update()
    {
        if(emit)
            if (lastSpawnTime + SPAWN_INTERVAL < Time.time)
            { // Is it time already for spawning a new particle?
                GameObject newLiquidParticle = (GameObject)Instantiate(Resources.Load("LiquidPhysics/DynamicParticle")); //Spawn a particle
                newLiquidParticle.GetComponent<Rigidbody2D>().AddForce(particleForce); //Add our custom force
                DynamicParticle particleScript = newLiquidParticle.GetComponent<DynamicParticle>(); // Get the particle script
                particleScript.SetLifeTime(PARTICLE_LIFETIME); //Set each particle lifetime
                particleScript.SetState(particlesState); //Set the particle State
                particleScript.SetColor(color); //Set the particle Color
                newLiquidParticle.transform.position = transform.position;// Relocate to the spawner position
                newLiquidParticle.transform.parent = particlesParent;// Add the particle to the parent container            
                lastSpawnTime = Time.time; // Register the last spawnTime            
            }
    }
}