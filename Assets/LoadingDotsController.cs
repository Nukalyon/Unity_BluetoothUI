using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingDotsController : MonoBehaviour
{
    [SerializeField] private GameObject _center;
    [SerializeField] private int _nbparticule = 10;
    [SerializeField] private GameObject _loadingDotsPrefab;
    private List<GameObject> _dots;
    private bool _loading = false;

    public float rotationSpeed = 60f;
    
    void Start()
    {
        _dots = new List<GameObject>();
        for (int i = 0; i < _nbparticule; i++)
        {
            GameObject newDot = Instantiate(_loadingDotsPrefab, gameObject.transform, true);
            Vector3 currentPos = newDot.transform.position;
            currentPos.x += 40;
            newDot.transform.position = currentPos;
            var step = (1-i) / _nbparticule;
            newDot.transform.localScale -= new Vector3(step, step, 0);
                // new Vector3(
                //     newDot.transform.localScale.x - step, 
                //     newDot.transform.localScale.y - step, 
                //     newDot.transform.localScale.z
                // );
            newDot.SetActive(false);
            _dots.Add(newDot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_loading)
        {
            foreach (GameObject dot in _dots)
            {
                dot.transform.RotateAround(_center.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                StartCoroutine(ALittleDelay());
            }
        }
    }

    IEnumerator ALittleDelay()
    {
        yield return new WaitForEndOfFrame();
    }

    private void OnEnable()
    {
        _dots.ForEach(dot => dot.SetActive(true));
        _loading = true;
    }

    private void OnDisable()
    {
        _dots.ForEach(dot => dot.SetActive(false));
        _loading = false;
    }
}
