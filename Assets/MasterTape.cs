using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class MasterTape : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMBombModule Module;
   public KMAudio Audio;
   public KMSelectable[] Buttons;
   public TextMesh[] DisplayTexts;
   public RotateReels scriptA;
   public RotateReels scriptA2;
   public WinAnimate scriptA3;

   //Wow this is dogshit
   public Transform TargetTapeOneChilds;
   public Transform TargetTapeTwoChilds;
   public Transform TargetTapeThreeChilds;
   public Transform PlayButton;


   public Material BlueButton;
   public Material RedButton;
   public Material LimeButton;
   public Material SmallButtons;

   int currentTapeStock = 0;
   int currentSpeed = 0;
   int currentSong = 0;

   int selectedSpeed = 0;
   int selectedTapeStock = 0;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */


      //Could merge these two. Would make more of a mess.
      //7 1/2 ips
      Buttons[0].OnInteract += delegate () { assign7(); return false; };
      //15 ips
      Buttons[1].OnInteract += delegate () { assign15(); return false; };

      //Play song
       Buttons[2].OnInteract += delegate () { playSong(); return false; };
      //Submit answers
      Buttons[3].OnInteract += delegate () { submitAnswer(); return false; };

      //TapeStocks
      Buttons[4].OnInteract += delegate () { assignFirstTapeStock(); return false; };
      Buttons[5].OnInteract += delegate () { assignSecTapeStock(); return false; };
      Buttons[6].OnInteract += delegate () { assignThirdTapeStock(); return false; };

Buttons[0].AddInteractionPunch();
Buttons[1].AddInteractionPunch();
Buttons[2].AddInteractionPunch();
Buttons[3].AddInteractionPunch();
Buttons[4].AddInteractionPunch();
Buttons[5].AddInteractionPunch();
Buttons[6].AddInteractionPunch();

   }

   void assign7() {
     if(selectedSpeed == 7) {
       //GET A JOB!!!
     }
     else {
        transform.GetChild(1).GetComponent<Renderer>().material = LimeButton;
        transform.GetChild(2).GetComponent<Renderer>().material = BlueButton;
        Debug.LogFormat("[Master Tapes " + ModuleId + "] Assigning 7 1/2 ips speed.", ModuleId);
        selectedSpeed = 7;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   void assign15() {
     if(selectedSpeed == 15) {
       //Good for nothing do nothing LOZER!
     }
     else {
        transform.GetChild(2).GetComponent<Renderer>().material = LimeButton;
        transform.GetChild(1).GetComponent<Renderer>().material = RedButton;
        Debug.LogFormat("[Master Tapes " + ModuleId + "] Assigning 15 ips speed.", ModuleId);
        selectedSpeed = 15;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   void playSong() {
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     scriptA.ShortSpin();
     scriptA2.ShortSpin();
     Debug.LogFormat("[Master Tapes " + ModuleId + "] Play button pressed. Song " + currentSong + " should be playing.", ModuleId);
     switch (currentSong)
        {
        case 6:
        Audio.PlaySoundAtTransform("Song6", Buttons[2].transform);
          break;
        case 5:
        Audio.PlaySoundAtTransform("Song5", Buttons[2].transform);
            break;
        case 4:
        Audio.PlaySoundAtTransform("Song4", Buttons[2].transform);
            break;
        case 3:
        Audio.PlaySoundAtTransform("Song3", Buttons[2].transform);
            break;
        case 2:
        Audio.PlaySoundAtTransform("Song2", Buttons[2].transform);
            break;
        case 1:
        Audio.PlaySoundAtTransform("Song1", Buttons[2].transform);
            break;
        default:
            break;
   }
}

   public void submitAnswer() {
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     if(!ModuleSolved) {
       //Write notes about what was right and wrong.
       Debug.LogFormat("[Master Tapes " + ModuleId + "] Submitted tape stock was: " + selectedTapeStock + ".", ModuleId);
       Debug.LogFormat("[Master Tapes " + ModuleId + "] Submitted tape speed was: " + selectedSpeed + ".", ModuleId);
       //Actual cases for songs being the correct ones
       //I just realized this entire switch case is unneccessary but It feels like it is for some reason... oh well... better coding next time...
       switch (currentSong)
           {
           case 6:
           if(selectedSpeed.Equals(currentSpeed) & selectedTapeStock.Equals(currentTapeStock))
           {
             OnSolve();
           }
           else {
             Strike();
             Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
             ClearColors();
           }
               break;
           case 5:
           if(selectedSpeed.Equals(currentSpeed) & selectedTapeStock.Equals(currentTapeStock))
           {
             OnSolve();
           }
           else {
             Strike();
             Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
             ClearColors();
           }
               break;
           case 4:
           if(selectedSpeed.Equals(currentSpeed) & selectedTapeStock.Equals(currentTapeStock))
           {
             OnSolve();
           }
           else {
             Strike();
             Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
             ClearColors();
           }
               break;
           case 3:
           if(selectedSpeed.Equals(currentSpeed) & selectedTapeStock.Equals(currentTapeStock))
           {
             OnSolve();
           }
           else {
             Strike();
             Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
             ClearColors();
           }
               break;
         case 2:
         if(selectedSpeed.Equals(currentSpeed) & selectedTapeStock.Equals(currentTapeStock))
         {
           OnSolve();
         }
         else {
           Strike();
           Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
           ClearColors();
         }
             break;
           case 1:
           if(selectedSpeed.Equals(currentSpeed) & selectedTapeStock.Equals(currentTapeStock))
           {
             OnSolve();
           }
           else {
             Strike();
             Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
             ClearColors();
           }
               break;
             }
     }
     else{
       //Do nothing. Why would we make a button do something if we already solved it? Omegalul.
     }
}


void ClearColors() {
  //Clears all inputs of colors. Gets rid of selections.
  TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
  TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
  TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
  transform.GetChild(1).GetComponent<Renderer>().material = RedButton;
  transform.GetChild(2).GetComponent<Renderer>().material = BlueButton;
  selectedSpeed = 0;
  selectedTapeStock = 0;
}

void WinColors() {
  //Lime greens all inputs of colors.
  TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
  TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
  TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
  transform.GetChild(1).GetComponent<Renderer>().material = LimeButton;
  transform.GetChild(2).GetComponent<Renderer>().material = LimeButton;
  PlayButton.GetComponent<Renderer>().material = LimeButton;
  transform.GetChild(5).GetComponent<Renderer>().material = LimeButton;
}

   void assignFirstTapeStock() {
     if(selectedTapeStock == 1) {
       //GET A JOB!!!
     }
     else {
       TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
        TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        Debug.LogFormat("[Master Tapes " + ModuleId + "] Assigning first tape stock (1).", ModuleId);
        selectedTapeStock = 1;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }
   void assignSecTapeStock() {
     if(selectedTapeStock == 2) {
       //Loooozer
     }
     else {
       TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
        TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        Debug.LogFormat("[Master Tapes " + ModuleId + "] Assigning second tape stock (2).", ModuleId);
        selectedTapeStock = 2;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   void assignThirdTapeStock() {
     if(selectedTapeStock == 3) {
       //snooozing on the job
     }
     else {
       TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
        Debug.LogFormat("[Master Tapes " + ModuleId + "] Assigning third tape stock (3).", ModuleId);
        selectedTapeStock = 3;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   void OnSolve() { //All the shit that happens when you solve the bitch
     scriptA.FastForward();
     scriptA2.FastForward();
     StartCoroutine(StartSolveAfterDelay());
     Audio.PlaySoundAtTransform("FastForward", Buttons[0].transform);
     DisplayTexts[0].text = "ARCHIVING...";
   }

   private System.Collections.IEnumerator StartSolveAfterDelay()
   {
       yield return new WaitForSeconds(3f); // Delay for 3 seconds
       scriptA.RestoreRotation();
       scriptA2.RestoreRotation();
       ActualSolve();
   }

   void ActualSolve() { //Glorious fun
     DisplayTexts[0].text = "CORRECT";
     WinColors();
     scriptA.WinSpin();
     scriptA2.WinSpin();
     Debug.LogFormat("[Master Tapes " + ModuleId + "] Submitting answers of " + selectedSpeed + " ips, and tape stock number " +selectedTapeStock+".", ModuleId);
     Audio.PlaySoundAtTransform("Solve", Buttons[2].transform);
     Solve();
     ModuleSolved = true;
     Debug.Log(ModuleSolved);
     scriptA3.WinAnimation();
   }

   void OnDestroy () { //Shit you need to do when the bomb ends

   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   void DetermineCorrectSong()
   {
    int randomNumber;
    randomNumber = UnityEngine.Random.Range(1, 7);
    currentSong = randomNumber;
   }

   void DetermineCorrectSpeed()
   {
     switch (currentSong)
         {
         case 6:
         currentSpeed = 7;
           break;
         case 5:
         currentSpeed = 15;
             break;
         case 4:
         currentSpeed = 7;
             break;
         case 3:
         currentSpeed = 15;
           break;
       case 2:
       currentSpeed = 7;
             break;
         case 1:
         currentSpeed = 15;
             break;
   }
}

   void DetermineCorrectTapeStock()
   {
     switch (currentSong)
         {
         case 6:
         currentTapeStock = 3;
           break;
         case 5:
         currentTapeStock = 1;
             break;
         case 4:
         currentTapeStock = 2;
             break;
         case 3:
         currentTapeStock = 2;
           break;
       case 2:
       currentTapeStock = 3;
             break;
         case 1:
         currentTapeStock = 1;
             break;
   }
   }

   void Start () { //Shit
     DisplayTexts[0].text = "SUBMIT"; //resets if someone fucks something up
     DetermineCorrectSong();
     DetermineCorrectSpeed();
     DetermineCorrectTapeStock();
     Debug.LogFormat("[Master Tapes " + ModuleId + "] Song number is " + currentSong + ".", ModuleId);
     Debug.LogFormat("[Master Tapes " + ModuleId + "] Correct speed is " + currentSpeed + ".", ModuleId);
     Debug.LogFormat("[Master Tapes " + ModuleId + "] Correct tape stock is " + currentTapeStock + ".", ModuleId);
   }

   void Update () { //Shit that happens at any point after initialization

   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
