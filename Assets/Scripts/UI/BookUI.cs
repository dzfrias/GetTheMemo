using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookUI : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject bookCopy;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        CreateBookCopy();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        Destroy(bookCopy);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        bookCopy.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        GameObject draggedObject = eventData.pointerDrag;
        Debug.Log(draggedObject);
        Debug.Log(gameObject);
        if (draggedObject.TryGetComponent(out BookUI _))
        {
            SwitchBookPositions(transform, draggedObject.transform);
        }
    }

    private void SwitchBookPositions(Transform book1, Transform book2)
    {
        Debug.Log("Switch book positions");
        Vector3 book1Position = book1.position;
        Vector3 book2Position = book2.position;

        book1.position = book2Position;
        book2.position = book1Position;
    }

    private void CreateBookCopy()
    {
        Debug.Log("Create Book Copy");
        bookCopy = Instantiate(
            gameObject,
            transform.position,
            Quaternion.identity,
            transform.parent.parent
        );
        Destroy(bookCopy.GetComponent<BookUI>());
        Image image = bookCopy.GetComponent<Image>();

        image.raycastTarget = false;
        float alphaAmount = 0.6f;
        image.color = image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            alphaAmount
        );
    }
}
