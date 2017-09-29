using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    public float FLOOR_HEIGHT = -2;
    public float BASE_GRAVITY = -0.05f;
    public float BUFFER = 1;

    public float gravity;
    public bool air;

    public bool stunned;
    public int stunTimer;

    public float vVelocity;
    public float hVelocity;

    public float speed;

    int texture;

    public static Texture2D[] textures;
    private string[] paths = {
        "\\textures\\attacktorb.png",
        "\\textures\\soldier.png"
    };

    public void loadTextures()
    {
        int numTextures = paths.Length;
        textures = new Texture2D[numTextures];
        for (int i = 0; i < numTextures; ++i)
        {
            textures[i] = (Texture2D)Resources.Load(paths[i]);
        }
    }

    // Use this for initialization
    void Start () {
        loadTextures();
        vVelocity = 0;
        gravity = BASE_GRAVITY;
        texture = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) && !air)
        {
            air = true;
            vVelocity = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 position = this.transform.position;
            position.x -= 0.04f;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //CROUCHING
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x += 0.04f;
            this.transform.position = position;
        }

        //PHYSICS

        vVelocity += gravity;

        moveY(vVelocity);
        
        if (this.transform.position.y < FLOOR_HEIGHT + BUFFER)
        {
            air = false;
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }

        if (texture==0)
        {
            setTexture(1);
        }
        else
        {
            setTexture(0);
        }
    }

    private void moveX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x += amm;
        this.transform.position = position;
    }

    private void moveY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y += amm;
        this.transform.position = position;
    }

    private void setY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y = amm;
        this.transform.position = position;
    }

    private void setX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x = amm;
        this.transform.position = position;
    }

    private float getY()
    {
        return this.transform.position.y;
    }

    private float getX()
    {
        return this.transform.position.x;
    }

    private void setTexture(int index)
    {
        Texture2D oldT = this.GetComponent<SpriteRenderer>().sprite.texture;
        Texture2D newT = textures[index];
        oldT = newT;
    }
}
