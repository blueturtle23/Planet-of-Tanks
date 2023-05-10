using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{

    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    // Start is called before the first frame update
    void Start()
    {
        //we need to know where we're reading from and writing to.
        // to help us with that we will print the current directory to the console.

        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is:" + currentDirectory);

        //load the scores by default.
        LoadScoresFromFile();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScoresFromFile()
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if(fileExists == true) 
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("The file " + scoreFileName+ "does not exist. No scores will be loaded.", this);

            return;
        }

        scores = new int[scores.Length];

        //now we read the file and use a streamreader which we give our full file path to.
        // dont forget the directory separator 
        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);

        //a counter so we dont go past the end of our scores
        int scoreCount = 0;

        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            string fileLine = fileReader.ReadLine();

            int readScore = -1;

            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                //if the number is read then put it in the array
                scores[scoreCount] = readScore;
            }
            else
            {
                //printing an error
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);

                scores[scoreCount] = 0;

            }

            scoreCount++;
        }

        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);

    }

    public void SaveScoresToFile()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);

        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        fileWriter.Close();

        Debug.Log("High scores written to " + scoreFileName);

    }

    public void AddScore(int newScore)
    {
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length;i++)
        {
            if (scores[i] > newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        if(desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this);

            return;
        }

        for(int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered into high scores at position " + desiredIndex, this);
    }
}
