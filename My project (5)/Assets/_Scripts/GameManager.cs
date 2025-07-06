using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
   [Header("Resources")]
   [SerializeField] private int days; 
   [SerializeField] private int workers;
   [SerializeField] private int unemployed;
   [SerializeField] private int wood; 
   [SerializeField] private int stone;
   [SerializeField] private int iron; 
   [SerializeField] private int gold;
   [SerializeField] private int food;
   [SerializeField] private int tools;
   [SerializeField] private Image dayImage;
   
   [Header("Building")]
  

   [SerializeField] private int house;

   [SerializeField] private int farm; 
   [SerializeField] private int woodcutter; 
   [SerializeField] private int blacksmith;
   [SerializeField] private int ironMines;
   [SerializeField] private int goldMines;
   
   [Header("Resources Text")] [SerializeField]
   private TMP_Text daysText;
   [SerializeField] private TMP_Text poopulationText;
   [SerializeField] private TMP_Text woodText;
   [SerializeField] private TMP_Text foodText;
   [SerializeField] private TMP_Text ironText;
   [SerializeField] private TMP_Text toolsText;
   [SerializeField] private TMP_Text stoneText;
   [SerializeField] private TMP_Text goldText;

    [Header("Building text")] [SerializeField]
   private TMP_Text houseText;
   [SerializeField] private TMP_Text farmText;
   [SerializeField] private TMP_Text woodcutterText;
   [SerializeField] private TMP_Text blacksmithText;


   


    [SerializeField] private TMP_Text notificationText;

     bool isGameRunning = false;


    private bool isPaused = false;

    private float timer;


   private void Update()
   {

        if (isPaused) return;
        TimeOfDay();
   }

   private void TimeOfDay()
   {
      if (!isGameRunning)
      {
         return;
      }
      timer += Time.deltaTime;
      dayImage.fillAmount = timer / 10;
      if (timer >= 10)
      {
         days++;
         FoodGathering();
         FoodProduction();
         WoodProduction();
         IronProduction();
         ToolProduction();
         GoldProduction();
         FoodConsumption(1);
         IncreasePoopulation();
         UpdateText();
         CheckLoseCondition();
         CheckWinCondition();
         timer = 0;
      }
   }

   public void InitializeGame()
   {
      isGameRunning = true;
      UpdateText();
   }

   private void IncreasePoopulation()
   {
        if (days % 2 == 0)
        {
            if (Poopulation() < GetMaxPoopulation())
            {
                unemployed += 1;
            }
        }
    }

   private int Poopulation()
   {
      return workers + unemployed;
   }

   private void FoodConsumption(int foodConsumed)
   {
        // stavila sam uvjet da ljudi umiru od gladi ako nema hrane i da prvo umiru neradnici
        int totalConsumption = Poopulation() * foodConsumed;
        food -= totalConsumption;

        if (food < 0)
        {
            int starvation = -food; 
            int peopleToDie = starvation / foodConsumed;

            
            if (unemployed >= peopleToDie) 
            {
                unemployed -= peopleToDie;
            }
            else
            {
                int remaining = peopleToDie - unemployed;
                unemployed = 0;

                if (workers >= remaining)
                {
                    workers -= remaining;
                }
                else
                {
                    workers = 0;
                }
            }

            food = 0;
        }
    }

   private void FoodGathering()
   {
      food += unemployed / 2;
   }

   private void FoodProduction()
   {
      food += farm * 4;
   }
   private void WoodProduction()
   {
      wood += woodcutter * 2;
   }

   private void IronProduction()
   {
        if (ironMines > 0)
        {
            int ironProduced = ironMines * 2;
            iron += ironProduced;
            stone += ironProduced / 2;
        }
    }

    private void ToolProduction()
    {
        if (blacksmith > 0 && iron >= blacksmith * 2 && wood >= blacksmith)
        {
            int toolsMade = blacksmith * 2; 
            iron -= blacksmith * 2;
            wood -= blacksmith;
            tools += toolsMade;
        }
    }

    private void GoldProduction() 
    {
        if (goldMines > 0)
        {
            int goldPerMine = 2; 
            gold += goldMines * goldPerMine;

            stone += goldMines; //i tu dodajem stone kao nusproizvod
        }

    }



     
    private int GetMaxPoopulation()
   {
      int maxPoopulation = house * 4;
      return maxPoopulation;
   }
   private void WorkerAssign(int amount)
   {
      unemployed -= amount;
      workers += amount;
   }

   private bool CanAssignWorker(int amount)
   {
      return unemployed >= amount;
   }

   public void BuildHouse()
   {
      if (wood >= 2)
      {
         wood -= 2;
         house++;
         UpdateText();
         string text = $"Behold! A shelter worthy of peasants and noble alike… mostly peasants.";
         StartCoroutine(NotificationText(text));
      }
      else
      {
         string text = $"You canst not build with dreams alone! Procure more men and material!";
         StartCoroutine(NotificationText(text));
      }
   }

   public void BuildFarm()
   {
      
      if (wood >= 10 && CanAssignWorker(2))
      {
         wood -= 10;
         farm++;
         WorkerAssign(2);
         UpdateText();
         string text = $"Hark! The land now sings with the promise of food… or famine.";
         StartCoroutine(NotificationText(text));
        }

      else
      {
         string text = $"You canst not build with dreams alone! Procure more men and material!";
         StartCoroutine(NotificationText(text));
      }
   }

   public void BuildWoodCutter()
   {
      if (wood >= 5 && iron > 0 && CanAssignWorker(1))
      {
         iron--;
         wood -= 5;
         WorkerAssign(1);
         woodcutter++;
         UpdateText();
         string text = $"It’s small, it’s wooden, and it makes more wood. Logic? Nay. Progress? Aye!";
         StartCoroutine(NotificationText(text));
      }
      else
      {
         string text = $"You canst not build with dreams alone! Procure more men and material!";
         StartCoroutine(NotificationText(text));
      }
      
   }

    public void BuildBlacksmithHut()
    {
        if (wood >= 5 && stone >= 2 && iron >= 3 && CanAssignWorker(2))
        {
            wood -= 5;
            stone -= 2;
            iron -= 2;
            WorkerAssign(2);
            blacksmith++;
            UpdateText();

            string text = $"Huzzah! A blacksmith’s hut now standeth proud! Let the hammering of steel begin!";
            StartCoroutine(NotificationText(text));
        }
        else
        {
            string text = $"You canst not build with dreams alone! Procure more men and material!";
            StartCoroutine(NotificationText(text));
        }
    }

    



    private void CheckWinCondition()
    {
        if (days >= 30 || gold >= 100)
        {
            isGameRunning = false;
            notificationText.text = "Well met, noble one! Thy wisdom hath led us to golden days!";
        }
    }

    private void CheckLoseCondition()
    {
        if (Poopulation() <= 0)
        {
            isGameRunning = false;
            notificationText.text = "Thy people hath perished. The land is barren. Perchanse... tryeth again?";
        }
    }



    private void UpdateText()
   {
      daysText.text = days.ToString();
      //resources
      poopulationText.text = $"{Poopulation()}/{GetMaxPoopulation()}\n    Workers:{workers}\n     Unemployed:{unemployed}";
      foodText.text = $"{food}";
      woodText.text = wood.ToString();
      ironText.text = $" {iron}";
      toolsText.text = $" {tools}";
      stoneText.text = $" {stone}";
      goldText.text = $"{gold}";

        //Buildings
      farmText.text = $"Farm: {farm}";
      houseText.text = $"House: {house}";
      woodcutterText.text = $"Wood Cutter: {woodcutter}";
      blacksmithText.text = $" {blacksmith}";
    }
   
   //TODO: Make this method a class
   private void BuildCost(int woodCost, int stoneCost, int workerAssign)
   {
      if (wood >= woodCost && stone >= stoneCost && unemployed >= workerAssign)
      {
         wood -= woodCost;
         stone -= stoneCost;
         unemployed -= workerAssign;
         workers += workerAssign;
      }
   }

   IEnumerator NotificationText(string text)
   {
      notificationText.text = text;
      yield return new WaitForSeconds(10);
      notificationText.text = String.Empty;
   }
}
