using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseUserManager : MonoBehaviour
    {
        public static List<UserData> global_userDatas;
        public bool didInit { get; set; }

        public void Init()
        {
            if (global_userDatas == null)
                global_userDatas = new List<UserData> ();

            didInit = true; 
        }

        public void ResetUsers()
        {
            if (!didInit)
                Init();

            global_userDatas = new List<UserData>();
        }

        public List<UserData> GetPlayerList()
        {
            if (global_userDatas == null)
                Init();

            return global_userDatas;
        }

        public int AddNewPlayer()
        {
            if (!didInit)
                Init();

            UserData newUser = new UserData();

            newUser.id = global_userDatas.Count;
            newUser.playerName = "Default";

            newUser.score = 0;
            newUser.health = 100;
            newUser.clones = 6;

            global_userDatas.Add(newUser);

            return newUser.id;
        }

        public string GetName(int id)
        {
            if (!didInit)
                Init();

            return global_userDatas[id].playerName;
        }

        public void SetName(int id, string aName)
        {
            if (!didInit)
                Init();

            global_userDatas[id].playerName = aName;
        }

        public int GetScore(int id)
        {
            if (!didInit)
                Init();

            return global_userDatas[id].score;
        }

        public void AddScore(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].score += anAmount;
        }

        public void ReduceScore(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].score -= anAmount;
        }

        public void SetScore(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].score = anAmount;
        }

        public int GetHealth(int id)
        {
            if (!didInit)
                Init();

            return global_userDatas[id].health;
        }

        public void AddHealth(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].health += anAmount;
        }

        public void ReduceHealth(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].health -= anAmount;
        }

        public void SetHealth(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].health = anAmount;
        }

        public int GetClones(int id)
        {
            if (!didInit)
                Init();

            return global_userDatas[id].clones;
        }

        public void AddClones(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].clones += anAmount;
        }

        public void ReduceClones(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].clones -= anAmount;
        }

        public void SetClones(int id, int anAmount)
        {
            if (!didInit)
                Init();

            global_userDatas[id].clones = anAmount;
        }
    }
}
