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
   public KMBombInfo BombInfo;
   public KMAudio Audio;
   public KMSelectable[] Buttons;
   public TextMesh[] DisplayTexts;
   public RotateReels scriptA;
   public RotateReels scriptA2;
   public WinAnimate scriptA3;

   //Wow this is dogshit
   public Renderer[] Speeds;
   public Renderer SubmitButton;
   public Transform TargetTapeOneChilds;
   public Transform TargetTapeTwoChilds;
   public Transform TargetTapeThreeChilds;
   public Transform PlayButton;


   public Material BlueButton;
   public Material RedButton;
   public Material LimeButton;
   public Material SmallButtons;

   private float flashDuration = 0.15f; // Duration of each flash in seconds
  private float totalDuration = 12f; // Total duration of the flashing effect in seconds
   int spamDuration = 10;
   int currentTapeStock = 0;
   int currentSpeed = 0;
   int currentSong = 0;
   int correctTime = 0;

   bool playingSong = false; //DONT SPAM THE SHITS!!!
   public Color realColor = new Color(178f / 255f, 246f / 255f, 166f / 255f);

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

//Yc0gKJ4u-TU
//Pyah!!!
Buttons[0].AddInteractionPunch();
Buttons[1].AddInteractionPunch();
Buttons[2].AddInteractionPunch();
Buttons[3].AddInteractionPunch();
Buttons[4].AddInteractionPunch();
Buttons[5].AddInteractionPunch();
Buttons[6].AddInteractionPunch();

   }

   void assign7() {
     if(selectedSpeed == 7 || ModuleSolved) {
       //GET A JOB!!!
     }
     else {
        Speeds[0].material = LimeButton;
        Speeds[1].material = BlueButton;
        Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Assigning 7 1/2 ips speed.", ModuleId);
        selectedSpeed = 7;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   void assign15() {
     if(selectedSpeed == 15 || ModuleSolved) {
       //Good for nothing do nothing LOZER!
     }
     else {
        Speeds[1].material = LimeButton;
        Speeds[0].material = RedButton;
        Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Assigning 15 ips speed.", ModuleId);
        selectedSpeed = 15;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   private System.Collections.IEnumerator SpamCoroutine()
   {
   	float startTime = Time.time;
   	while (Time.time - startTime < spamDuration)
   	{
   			yield return null; // Wait for the next frame
   	}
    playingSong = false;
   }

   void playSong() {
     if(!playingSong && !ModuleSolved){
     playingSong = true;
     StartCoroutine(SpamCoroutine());
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[2].transform);
     scriptA.ShortSpin();
     scriptA2.ShortSpin();
     Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Play button pressed. Song " + currentSong + " should be playing.", ModuleId);
     switch (currentSong)
        {
          case 9:
          Audio.PlaySoundAtTransform("Song9", Buttons[2].transform);
            break;
            case 8:
            Audio.PlaySoundAtTransform("Song8", Buttons[2].transform);
              break;
          case 7:
          Audio.PlaySoundAtTransform("Song7", Buttons[2].transform);
            break;
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
 else{
   //donothingcuzlike...whyspamthebutton
 }
}

public void submitAnswer()
{
    if (!ModuleSolved)
    {
        //Removed that godawful extra 175 lines of code of switches that all did the same shit.
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
        switch (currentSong)
        {
            case 9:
            case 8:
            case 7:
            case 6:
            case 5:
            case 4:
            case 3:
            case 2:
            case 1:
                if (selectedSpeed.Equals(currentSpeed) && selectedTapeStock.Equals(currentTapeStock) && (int)BombInfo.GetTime() % 10 == correctTime)
                {
                    OnSolve();
                }
                else
                {
                    if ((int)BombInfo.GetTime() % 10 != correctTime)
                    {
                        Debug.LogFormat("[Master Tapes #{0}] Strike! Defuser entered incorrect time! The correct time to press was: {1}", ModuleId, correctTime);
                    }
                    else
                    {
                        Debug.LogFormat("[Master Tapes #{0}] Strike! Defuser entered {1} ips and tape stock {2} instead of {3} ips and tape stock {4}.", ModuleId, selectedSpeed, selectedTapeStock, currentSpeed, currentTapeStock);
                    }
                    Strike();
                    Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
                    ClearColors();
                }
                break;
        }
    }
    else
    {
        // Do nothing. Module is already solved.
    }
}



void ClearColors() {
  //Clears all inputs of colors. Gets rid of selections.
  TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
  TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
  TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
  Speeds[0].material = RedButton;
  Speeds[1].material = BlueButton;
  selectedSpeed = 0;
  selectedTapeStock = 0;
}

void WinColors() {
  //Lime greens all inputs of colors.
  TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
  TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
  TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
  Speeds[0].material = LimeButton;
  Speeds[1].material = LimeButton;
  PlayButton.GetComponent<Renderer>().material = LimeButton;
  SubmitButton.material = LimeButton;
}

   void assignFirstTapeStock() {
     if(selectedTapeStock == 1 || ModuleSolved) {
       //GET A JOB!!!
     }
     else {
       TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
        TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Assigning tape stock 1.", ModuleId);
        selectedTapeStock = 1;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }
   void assignSecTapeStock() {
     if(selectedTapeStock == 2 || ModuleSolved) {
       //Loooozer
     }
     else {
       TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
        TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Assigning tape stock 2.", ModuleId);
        selectedTapeStock = 2;
        Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     }
   }

   void assignThirdTapeStock() {
     if(selectedTapeStock == 3 || ModuleSolved) {
       //snooozing on the job
     }
     else {
       TargetTapeOneChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeTwoChilds.GetChild(0).GetComponent<Renderer>().material = SmallButtons;
        TargetTapeThreeChilds.GetChild(0).GetComponent<Renderer>().material = LimeButton;
        Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Assigning tape stock 3.", ModuleId);
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

   private System.Collections.IEnumerator FlashText()
   {
     float elapsedTime = 0f;
     while (elapsedTime < totalDuration)
     {
         DisplayTexts[0].color = Color.white; // Flash to white
         DisplayTexts[1].color = Color.white; // Flash to white
         yield return new WaitForSeconds(flashDuration);
         DisplayTexts[0].color = realColor; // Flash to unityColor
         DisplayTexts[1].color = realColor; // Flash to white
         yield return new WaitForSeconds(flashDuration);
         elapsedTime += flashDuration * 2f;
     }
   }

   void ActualSolve() { //Glorious fun
     StartCoroutine(FlashText());
     DisplayTexts[0].text = "CORRECT";
     WinColors();
     scriptA.WinSpin();
     scriptA2.WinSpin();
     Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Solved! Answers were " + selectedSpeed + " ips, and tape stock number " +selectedTapeStock+". " + "Correct speed was pressed at: " + correctTime +".", ModuleId);
     Audio.PlaySoundAtTransform("Solve", Buttons[2].transform);
     Solve();
     ModuleSolved = true;
     scriptA3.WinAnimation();
   }

   void OnDestroy () { //Shit you need to do when the bomb ends

   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   void DetermineCorrectTime()
{
    if (Bomb.IsPortPresent(Port.StereoRCA))
        correctTime = 5;
    else if (Bomb.GetBatteryCount() % 2 == 0)
        correctTime = 4;
    else if (Bomb.IsPortPresent(Port.Parallel))
        correctTime = 3;
    else if (Bomb.GetSerialNumberNumbers().Any(n => n % 2 == 0))
        correctTime = 2;
    else
        correctTime = 1;
}

   void DetermineCorrectSong()
   {
    int randomNumber;
    randomNumber = UnityEngine.Random.Range(1, 10);
    currentSong = randomNumber;
   }

   void DetermineCorrectSpeed()
   {
     switch (currentSong)
         {
           case 9:
           currentSpeed = 7;
             break;
           case 8:
           currentSpeed = 15;
             break;
           case 7:
           currentSpeed = 15;
             break;
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
           case 9:
           currentTapeStock = 3;
             break;
           case 8:
           currentTapeStock = 1;
             break;
           case 7:
           currentTapeStock = 2;
             break;
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
     DetermineCorrectTime();
     DetermineCorrectSong();
     DetermineCorrectSpeed();
     DetermineCorrectTapeStock();
     Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Song index number is " + currentSong + ".", ModuleId);
     Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Correct speed is " + currentSpeed + " ips.", ModuleId);
     Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Correct tape stock is #" + currentTapeStock + ".", ModuleId);
     Debug.LogFormat("[Master Tapes " + "#" + ModuleId + "] Correct time to submit answers is " + correctTime + ".", ModuleId);
   }

   void Update () { //Shit that happens at any point after initialization

   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

//THE BANE OF ALL EXISTENCE.... THE TWITCH PLAYS CODE...
//Reworked entirely from the Girlfriend module. Many thanks.
//Thanks VFlyer for new help message YAYAYAYA.
//https://www.youtube.com/watch?v=VUtX0ZKlwis WORD UP BIRD UP
#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use `!{0} play` to play the song on the tape. Use `!{0} tape [1,2,3]` to select the tape stock from top to bottom. Use `!{0} speed [7 1/2,15]` to select the tape speed. Use `!{0} submit at #` to submit when the last seconds digit of the countdown timer is that value.";
#pragma warning restore 414

IEnumerator ProcessTwitchCommand (string Command) {

     Command = Command.ToUpper();
     yield return null;

     switch (Command)
     {
         case "PLAY":
             Buttons[2].OnInteract();
             break;

          case "TAPE 1":
              Buttons[4].OnInteract();
              break;
          case "TAPE 2":
              Buttons[5].OnInteract();
              break;
          case "TAPE 3":
              Buttons[6].OnInteract();
              break;

          case "SPEED 7 1/2":
              Buttons[0].OnInteract();
              break;

          case "SPEED 15":
              Buttons[1].OnInteract();
              break;


//Using all the same convuluted script. Works as it intends. Too lazy to change. Stolen from Color-Cycle. SUE ME!!!
//This just submits it if its within the correct time, not the specified time...
case "SUBMIT AT 1":
  int submissionnum = 1;
  while ((int)BombInfo.GetTime() % 10 == submissionnum)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 2":
  int submissionnum1 = 2;
  while ((int)BombInfo.GetTime() % 10 == submissionnum1)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum1)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 3":
  int submissionnum2 = 3;
  while ((int)BombInfo.GetTime() % 10 == submissionnum2)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum2)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 4":
  int submissionnum3 = 4;
  while ((int)BombInfo.GetTime() % 10 == submissionnum3)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum3)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 5":
  int submissionnum4 = 5;
  while ((int)BombInfo.GetTime() % 10 == submissionnum4)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum4)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 6":
  int submissionnum5 = 6;
  while ((int)BombInfo.GetTime() % 10 == submissionnum5)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum5)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 7":
  int submissionnum6 = 8;
  while ((int)BombInfo.GetTime() % 10 == submissionnum6)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum6)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 8":
  int submissionnum7 = 9;
  while ((int)BombInfo.GetTime() % 10 == submissionnum7)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum7)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

case "SUBMIT AT 9":
  int submissionnum8 = 9;
  while ((int)BombInfo.GetTime() % 10 == submissionnum8)
    yield return "trycancel";  // Fixes a really obscure bug with tp
  while ((int)BombInfo.GetTime() % 10 != submissionnum8)
    yield return "trycancel";
  Buttons[3].OnInteract();
  yield return new WaitForSeconds(0.1f);
  break;

         default:
                 yield return string.Format("sendtochaterror Invalid command or cannot press button currently");
                 yield break;
     }
}

   IEnumerator TwitchHandleForcedSolve () {
       int TwitchSpeed = currentSpeed;
       int TwitchTapeStock = currentTapeStock;
       int TwitchSong = currentSong;
       switch(TwitchSpeed)
       {
         case 7:
         Buttons[0].OnInteract();
         break;
         case 15:
         Buttons[1].OnInteract();
         break;
         default:
                 yield return string.Format("sendtochaterror Cannot assign speed");
                 yield break;
       }
       switch(TwitchSong)
       {
         //4 is Scotch
         //5 is BASF
         //6 is Ampex
         case 9:
         Buttons[6].OnInteract();
           break;
         case 8:
         Buttons[4].OnInteract();
           break;
         case 7:
         Buttons[5].OnInteract();
           break;
         case 6:
         Buttons[6].OnInteract();
           break;
         case 5:
         Buttons[4].OnInteract();
             break;
         case 4:
         Buttons[5].OnInteract();
             break;
         case 3:
         Buttons[5].OnInteract();
           break;
       case 2:
       Buttons[6].OnInteract();
             break;
         case 1:
         Buttons[4].OnInteract();
             break;
       }
       int twitchTimer = correctTime;
       string twitchTimerCommand = twitchTimer.ToString();
       yield return ProcessTwitchCommand("submit at " + twitchTimerCommand);
   }
}
