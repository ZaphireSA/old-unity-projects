using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact();
    void Interact(Player player);
    void Interact(Vector3 position);
}
