using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Skeletom.Essentials.Lifecycle {

    /// <summary>
    /// An object pool which allows for quick recycling of objects instead of more costly instantiation/destruction.
    /// New objects will be instantiated and added to the pool as needed if the pool is exhausted.
    /// Useful for when you have things like particles, projectiles, or other obejcts that may need to be summoned in large quantities rapidly.
    /// </summary>
    /// <typeparam name="T">The type of object that this pool is for.</typeparam>
	public class ObjectPool<T> where T : UnityEngine.Component {

		private readonly List<T> _active = new List<T>();
		public IEnumerable<T> ActiveObjects { get { return new ReadOnlyCollection<T>(this._active); } }
		private readonly Queue<T> _inactive = new Queue<T>();
		private readonly T _prefab = null;
		private readonly Transform _parent = null;
		private readonly int _initialSize = 0;

        /// <summary>
        /// Creates an object pool based on a given prefab, filling the pool with the specified quantity of objects initially.
        /// </summary>
        /// <param name="prefab">The prefab to make copies of.</param>
        /// <param name="initialSize">The number of objects in the pool.</param>
        /// <param name="parent">Where to parent the objects to when not in use.</param>
		public ObjectPool(T prefab, int initialSize, Transform parent) {
			this._initialSize = initialSize;
			this._prefab = prefab;
			this._parent = parent;
			for (int i = 0; i < this._initialSize; i++) {
				RetireInternal(Instantiate());
			}
		}

		private T Instantiate() {
			return MonoBehaviour.Instantiate<T>(this._prefab, Vector3.zero, Quaternion.identity);
		}

		private void RetireInternal(T instance) {
			instance.transform.SetParent(this._parent);
			instance.transform.localPosition = Vector3.zero;
			instance.gameObject.SetActive(false);

			this._inactive.Enqueue(instance);
		}

		public T GetNext() {
			if (this._inactive.Count <= 0) {
				RetireInternal(Instantiate());
			}
			T instance = this._inactive.Dequeue();
			instance.gameObject.transform.SetParent(null);
			instance.gameObject.SetActive(true);
			this._active.Add(instance);
			return instance;
		}

		public void Retire(T instance) {
			if (this._active.Contains(instance)) {
				this._active.Remove(instance);
				RetireInternal(instance);
			}
		}

		public void CullExcess() {
			if (this._active.Count + this._inactive.Count > this._initialSize
			&& this._inactive.Count > 0) {
				do {
					MonoBehaviour.Destroy(this._inactive.Dequeue().gameObject);
				} while (this._active.Count + this._inactive.Count > this._initialSize);
			}
		}
	}
}