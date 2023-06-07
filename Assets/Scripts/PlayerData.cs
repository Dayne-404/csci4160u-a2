using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    public string name;
    public string baseMaterial;
    public string accessoryMaterial;

    public PlayerData(string name, string baseMaterial, string accessoryMaterial) {
        this.name = name;
        this.baseMaterial = baseMaterial;
        this.accessoryMaterial = accessoryMaterial;
    }

    public override string ToString() {
        string s = "";
        s += "Name: " + name;
        s += " Base: " + baseMaterial;
        s += " AccessoryMat: " + accessoryMaterial;
        return s;
    }
}
