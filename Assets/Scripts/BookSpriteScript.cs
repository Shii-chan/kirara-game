using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSpriteScript : MonoBehaviour
{
    Image imageComponent;

    public Sprite closedBookSprite;    
    public Sprite openBookSprite;    
    // Start is called before the first frame update
    void Start()
    {
        this.imageComponent = GetComponent<Image>();
        this.setClosedBook();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setClosedBook(){
        this.imageComponent.sprite = this.closedBookSprite;
    }

    public void setOpenBook(){
        this.imageComponent.sprite = this.openBookSprite;
    }
}
