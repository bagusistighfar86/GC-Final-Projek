using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KartAgent : Agent
{
   public CheckpointManager _checkpointManager;
   private KartController _kartController;
   // float countDown = 5.0f;
   
   //Dipanggil sekali saat start
   public override void Initialize()
   {
      _kartController = GetComponent<KartController>();
   }
   
   //Dipanggil saat waktu habis dan telah mencapai goal
   public override void OnEpisodeBegin()
   {
      // transform.position = _spawnPosition.position + new Vector3(Random.Range(-5f, +5f),0, Random.Range(-5f, +5f));
      // transform.forward = _spawnPosition.forward;
      _checkpointManager.ResetCheckpoints();
      _kartController.Respawn();
   }

   void Update () {
      // countDown -= Time.deltaTime;

      // if(countDown > 0){
      //    Debug.Log("ARAH BENAR");
      //    transform.position += Vector3.forward * Time.deltaTime;
      // }
      // else if (countDown <= 0.0f){ // You don't want to move backwards too much!
      //    Debug.Log("SALAHHH ARAH");
      //    transform.position += Vector3.back * Time.deltaTime;
      // } 
 }

   #region Edit this region!

      //Mengumpulkan informasi tambahan yang tidak diambil oleh RaycastSensors
      public override void CollectObservations(VectorSensor sensor)
      {
         // Vector3 toTarget = (_checkpointManager.nextCheckPointToReach.transform.position - transform.position).normalized;
         // if (Vector3.Dot(toTarget, transform.forward) > 0) {
         //    Debug.Log("Target is in front of this game object.");
         // } else {
         //    Debug.Log("Target is not in front of this game object.");
         // }
         Vector3 checkpointForward = _checkpointManager.nextCheckPointToReach.transform.forward;
         float directionDot = Vector3.Dot(transform.forward, checkpointForward);
         sensor.AddObservation(directionDot);

         // Vector antara Kart dengan Checkpoint
         Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
         sensor.AddObservation(diff / 20f);

         AddReward(-0.001f); // Mempercepat kecepatan menyetir
      }

      //Proses aksi diterima
      public override void OnActionReceived(ActionBuffers actions)
      {
         var input = actions.ContinuousActions;

         _kartController.ApplyAcceleration(input[1]);
         _kartController.Steer(input[0]);

         // Agar terus menambah kecepatan diberikan reward kecil sesuai kecepatannya
         
      }
      
      //For manual testing with human input, the actionsOut defined here will be sent to OnActionRecieved
      public override void Heuristic(in ActionBuffers actionsOut)
      {
         var action = actionsOut.ContinuousActions;
         
         action[0] = Input.GetAxis("Horizontal"); //Steering

         if (Input.GetKey(KeyCode.W)){
            action[1] = 1f;
         }
         else {
            action[1] = 0f;
         }
      }

   #endregion
}
