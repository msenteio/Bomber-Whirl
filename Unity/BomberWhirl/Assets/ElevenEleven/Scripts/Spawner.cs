/// <summary>
/// Copyright (c) 2016 11:11 Studios LLC
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this 
/// software and associated documentation files (the "Software"), to deal in the Software 
/// without restriction, including without limitation the rights to use, copy, modify, merge, 
/// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
/// to whom the Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included 
/// in all copies or substantial portions of the Software.
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
/// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
/// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
/// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ElevenEleven {
    public class Spawner : MonoBehaviour {

        [SerializeField]
        Transform[] toSpawn;
        [SerializeField]
        bool randomAngle;
        [SerializeField]
        int initiallySpawned;
        [SerializeField]
        int maxAllowedSpawned;
        [SerializeField]
        float startSpawnDelay;
        [SerializeField]
        float minSpawnTime;
        [SerializeField]
        float maxSpawnTime;
        [SerializeField]
        Bounds bounds;
        [SerializeField]
        int bagDuplicateCount = 2;

        List<Transform> spawnBag = new List<Transform>();

        IEnumerator Start() {

            for (int i = 0; i < initiallySpawned; i++) {
                Spawn();
            }

            yield return new WaitForSeconds(startSpawnDelay);

            if (minSpawnTime > 0.0f || maxSpawnTime > 0.0f) {
                do {
                    yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
                    if (transform.childCount < maxAllowedSpawned) {
                        Spawn();
                    }
                } while (true);
            }
        }

        void Spawn() {
            if (spawnBag.Count == 0) {
                foreach (var item in toSpawn) {
                    for (int i = 0; i < bagDuplicateCount; i++) {
                        spawnBag.Add(item);
                    }
                }
            }

            int index = Random.Range(0, spawnBag.Count);
            Transform prefab = spawnBag[index];
            spawnBag.RemoveAt(index);

            Transform instance = (Transform)Instantiate(prefab);
            instance.parent = transform;
            instance.localPosition = ElevenTools.RandomPoint(bounds);
            instance.localRotation = Quaternion.identity;
        }
    }
}