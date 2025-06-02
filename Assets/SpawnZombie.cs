using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnZombie : MonoBehaviour
{
   public GameObject zombie;
   public GameObject pj1, pj2;

   private void Update()
   {
      if (Input.GetKeyUp(KeyCode.L))
      {
         OnSpawnZombie();
      }

      if (Input.GetKeyUp(KeyCode.R))
      {
         Revivir();
      }

      if (Input.GetKeyUp(KeyCode.Alpha2))
      {
         CambiarPersonaje();
      }

      if (Input.GetKeyUp(KeyCode.T))
      {
         Reiniciar();
      }
   }

   public void OnSpawnZombie()
   {
      Instantiate(zombie, transform.position, transform.rotation);
   }

   public void CambiarPersonaje()
   {
      pj1.SetActive(pj1.activeSelf? false: true);
      pj2.SetActive(pj2.activeSelf? false: true);

      foreach (GameObject enemigo in GameObject.FindGameObjectsWithTag("Enemy"))
      {
         enemigo.GetComponentInParent<EnemyAI>().BuscarJugador();
      }
   }

   public void Reiniciar()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }

   public void Revivir()
   {
      pj1.GetComponentInChildren<HealthSystem>().Revivir();
      pj2.GetComponentInChildren<HealthSystem>().Revivir();
   }
}
