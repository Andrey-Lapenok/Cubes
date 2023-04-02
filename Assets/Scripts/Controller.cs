using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Controller : MonoBehaviour
{
	public List<string> commands = new List<string>();
	private int a = 0;
	private float time = 0;

    public void Action() {
        string[] arr = ReadCommand(commands[a]).ToArray();
        switch(arr[0]) {
            case "Open": Open(arr[1]); break;
            case "Close": Close(arr[1]); break;
            case "Pass": Pass(); break;
            case "Transfer": Transfer(Convert.ToInt32(arr[1]), Convert.ToInt32(arr[2])); break;
        }
        a++;
        if (a >= commands.Count) a = 0;
    }

    private List<string> ReadCommand(string s) {
        List<string> commands = new List<string>();
        string argument = "";
        bool a = false;
        for (int i = 0; i < s.Length; i++) {
            if (s[i] == '(') {a = true; continue;}
            else if (s[i] == ')') {a = false; commands.Add(argument); argument = ""; continue;}

            if (a) argument += s[i];
        }
        return commands;
    }

    void Pass() {}

    void Transfer(int n, int count) {
        a = n;
        for (int i = 0; i < count; i++) Action();
    }

    void Open(string name) {
        GameObject.Find(name).GetComponent<DoorController>().Open();
    }

    void Close(string name) {
        GameObject.Find(name).GetComponent<DoorController>().Close();
    }
}
