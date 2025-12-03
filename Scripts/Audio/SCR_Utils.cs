using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SCR_Utils 
{
   public static void RunAfterDelay(MonoBehaviour monoBehaviour, float dealy, Action task) {
      monoBehaviour.StartCoroutine(RunAfterDelayRoutine(dealy, task));
   }

   private static IEnumerator RunAfterDelayRoutine(float delay, Action task) {
      yield return new WaitForSeconds(delay);
      task?.Invoke();
   }
}
