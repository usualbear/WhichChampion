﻿using System;
using System.Collections.Generic;
using System.IO;
using GLib;
using YamlDotNet.Serialization;

namespace ChampionSelector
{    
    class Program
    {
        static void Main(string[] args)
        {
            Util util = new Util();
            
            //Construct API call
            APIMessage apiCall = new APIMessage();
            ChampionDto champData = apiCall.MakeRequest();
            Champions champs = new Champions(champData);
            
            //Construct question list
            Questions questionList = new Questions(Document);
            
            //Holds answers to questions
            Answer ans = new Answer();
            //Ask questions
            foreach(QuestionObj q in questionList.questions)
            {
                Console.WriteLine(q.question);
                foreach (string answer in q.answers)
                {
                    Console.WriteLine(answer);
                }
                string input = (Console.ReadLine());
                LogQuestion(ans, q.symbol, input);
            }
            Console.WriteLine(champs.FilterCrewByCriteria(ans));
        }

        private static void LogQuestion(Answer ans, string questionSymbol, string answer)
        {
            if (questionSymbol == "question_1")
            {
                Lane.TryParse(answer, out Lane lane);
                ans.lane = lane;
            }
            else if (questionSymbol == "question_2")
            {
                Role.TryParse(answer, out Role role);
                ans.role = role;
            }
            else if (questionSymbol == "question_3")
            {
                DamageType.TryParse(answer, out DamageType dmg);
                ans.dmgType = dmg;
            }
            else if (questionSymbol == "question_4")
            {
                IsNew.TryParse(answer, out IsNew n);
                ans.isNew = n;
            }
        }
        
        private const string Document = @"---
        questions:
            - symbol: question_1
              question: What lane do you feel like playing?
              answers:
                - Top
                - Jungle
                - Mid
                - Bottom
                - Support
            - symbol: question_2
              question: What style do you want to play?
              answers:
                - Ranged
                - Bruiser
                - Tank
            - symbol: question_3
              question: Preference on damage type?
              answers:
                - AP
                - AD
                - Hybrid
                - Don't Care
            - symbol: question_4
              question: Do you want to try something new, or play something you know?
              answers:
                - Try something new!
                - Give me something familiar
                - I really don't care
...";
    }
}