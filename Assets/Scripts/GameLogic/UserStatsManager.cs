using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserStatsManager : MonoBehaviour
    {
        public BaseUserManager _myDataManager;
        public int myID { get; set; }
        public bool didInit { get; set; }
        public bool disableAutoPlayerListAdd;

        private void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            Debug.Log("Init Player Controller");
            SetupDataManager();
            didInit = true;
        }

        public virtual void SetPlayerDetails(int anID)
        {
            // this function can be used by a game manager to pass in details from the player list, such as
            // this players ID or perhaps you could override this in the future to add avatar support, loadouts or
            // special abilities etc?
            myID = anID;
        }

        public virtual void SetupDataManager()
        {
            // if a player manager is not set in the editor, let's try to find one
            if (_myDataManager == null)
                _myDataManager = GetComponent<BaseUserManager>();

            if (_myDataManager == null)
                _myDataManager = gameObject.AddComponent<BaseUserManager>();

            if (_myDataManager == null)
                _myDataManager = GetComponent<BaseUserManager>();
        }

        public string GetName()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetName(myID);
        }

        public virtual void SetName(string aName)
        {
            if (!didInit)
                Init();

            _myDataManager.SetName(myID, aName);
        }

        public int GetScore()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetScore(myID);
        }

        public virtual void AddScore(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.AddScore(myID, anAmount);
        }    

        public virtual void ReduceScore(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.ReduceScore(myID, anAmount);
        }
        
        public virtual void SetScore(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.SetScore(myID, anAmount);
        }

        public int GetHealth()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetHealth(myID);
        }

        public virtual void AddHealth(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.AddHealth(myID, anAmount);
        }

        public virtual void ReduceHealth(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.ReduceHealth(myID, anAmount);
        }

        public virtual void SetHealth(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.SetHealth(myID, anAmount);
        }

        public int GetClones()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetClones(myID);
        }

        public virtual void AddClones(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.AddClones(myID, anAmount);
        }

        public virtual void ReduceClones(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.ReduceClones(myID, anAmount);
        }

        public virtual void SetClones(int anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.SetClones(myID, anAmount);
        }
    }
}
