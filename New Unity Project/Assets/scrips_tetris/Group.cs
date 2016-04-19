﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Group : MonoBehaviour {
	float lastFall =0;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update() {
		// Move Left
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			moveLeft ();
		}
		// Move Right
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			moveRight ();
		}
		// Rotate
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Rotate ();
		}
		// Fall
		else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Fall ();
		} 
		//restart
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			restart ();
		}
	

		// Move Downwards and Fall
		else if (Input.GetKeyDown(KeyCode.DownArrow) ||
			Time.time - lastFall >= 1) {
			// Modify position
			transform.position += new Vector3(0, -1, 0);

			// See if valid
			if (isValidGridPos()) {
				// It's valid. Update grid.
				updateGrid();
			} else {
				// It's not valid. revert.
				transform.position += new Vector3(0, 1, 0);

				// Clear filled horizontal lines
				Grid.deleteFullRows();

				// Spawn next Group
				FindObjectOfType<Spawner>().spawnNext();

				// Disable script
				enabled = false;
			}

			lastFall = Time.time;
		}
	}
	bool isValidGridPos() {        
		foreach (Transform child in transform) {
			Vector2 v = Grid.roundVec2(child.position);

			// Not inside Border?
			if (!Grid.insideBorder(v))
				return false;

			// Block in grid cell (and not part of same group)?
			if (Grid.grid[(int)v.x, (int)v.y] != null &&
				Grid.grid[(int)v.x, (int)v.y].parent != transform)
				return false;
		}
		return true;
	}
	void updateGrid() {
		// Remove old children from grid
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.w; ++x)
				if (Grid.grid[x, y] != null)
				if (Grid.grid[x, y].parent == transform)
					Grid.grid[x, y] = null;

		// Add new children to grid
		foreach (Transform child in transform) {
			Vector2 v = Grid.roundVec2(child.position);
			Grid.grid[(int)v.x, (int)v.y] = child;
		}        
	}
	public void restart(){
		SceneManager.LoadScene ("Tetris 1");
	}

	public  void moveLeft(){
		// Modify position
		transform.position += new Vector3 (-1, 0, 0);

		// See if valid
		if (isValidGridPos ())
			// Its valid. Update grid.
			updateGrid ();
		else
			// Its not valid. revert.
			transform.position += new Vector3 (1, 0, 0);
	}
	public void moveRight(){
		// Modify position
		transform.position += new Vector3(1, 0, 0);

		// See if valid
		if (isValidGridPos())
			// It's valid. Update grid.
			updateGrid();
		else
			// It's not valid. revert.
			transform.position += new Vector3(-1, 0, 0);
	}
	public void Rotate(){
		transform.Rotate(0, 0, -90);

		// See if valid
		if (isValidGridPos())
			// It's valid. Update grid.
			updateGrid();
		else
			// It's not valid. revert.
			transform.Rotate(0, 0, 90);
	
	}
	public void Fall(){
		// Modify position
		transform.position += new Vector3(0, -1, 0);

		// See if valid
		if (isValidGridPos()) {
			// It's valid. Update grid.
			updateGrid();
		} else {
			// It's not valid. revert.
			transform.position += new Vector3(0, 1, 0);

			// Clear filled horizontal lines
			Grid.deleteFullRows();

			// Spawn next Group
			FindObjectOfType<Spawner>().spawnNext();

			// Disable script
			enabled = false;
		}
		}

}
