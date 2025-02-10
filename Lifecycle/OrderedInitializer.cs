using System;
using UnityEngine;

namespace Skeletom.Essentials.Lifecycle {

    /// <summary>
    /// Class for objects that exist on launch and need to be loaded in a specific order 
    // (IE, set up UI before loading Save Data, etc)
    /// </summary>
    public abstract class OrderedInitializer : MonoBehaviour, IComparable {
        [Tooltip("The order in which the Initialize method will be called. Larger values go later.")]
        [SerializeField]
        private int _initializationOrder;

        /// <summary>
        /// Method to do ordered loading within
        /// 
        /// Will be called after Awake()
        /// </summary>
        public abstract void Initialize();

        private static bool LOADING_STARTED = false;
        private static bool LOADING_FINISHED = false;

        private void Awake() {
            // the first object to run Start will Initialize all of the currently extant 
            if (!LOADING_STARTED) {
                LOADING_STARTED = true;
                OrderedInitializer[] objects = FindObjectsOfType<OrderedInitializer>();
                Array.Sort(objects);
                foreach (OrderedInitializer o in objects) {
                    Debug.LogFormat("Initializing {0}", o.name);
                    o.Initialize();
                }
                LOADING_FINISHED = true;
            }
            if(LOADING_FINISHED) {
                this.Initialize();
            }
        }

        public int CompareTo(object obj) {
            try {
                OrderedInitializer o = (OrderedInitializer)obj;
                return _initializationOrder - o._initializationOrder;
            }
            catch {
                return 0;
            }
        }
    }
}