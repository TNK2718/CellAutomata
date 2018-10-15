using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources_Loader : MonoBehaviour {

    string[] rule_raw; //テキストの加工前の一行を入れる変数 rule
    string[] rule_raw2;
    string[] states_raw;//states
    string[] states_raw2;
    string[] cells_raw;
    string[] cells_raw2;


    public string[,] rule_parameters; //テキストの複数列を入れる2次元配列 
    public string[,] rule_parameters2;
    public string[,] states_parameters;
    public string[,] states_parameters2;
    public string[,] cells_ini;
    public string[,] cells_another_ini;


    private int rowLength; //テキスト内の行数を取得する変数
    private int columnLength; //テキスト内の列数を取得する変数
    private int rowLength2;
    private int columnLength2;
    private int states_length;
    private int states_length2;
    private int func_length;
    private int func_length2;
    private int cells_row;
    private int cells_column;
    private int cells_row2;
    private int cells_column2;

    private void Start()
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        TextAsset textasset2 = new TextAsset();
        TextAsset txt3 = new TextAsset();
        TextAsset txt4 = new TextAsset();
        TextAsset txt5 = new TextAsset();
        TextAsset txt6 = new TextAsset();

        textasset = Resources.Load("Rules", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        textasset2 = Resources.Load("Rules_another", typeof(TextAsset)) as TextAsset;
        txt3 = Resources.Load("Cell_func", typeof(TextAsset)) as TextAsset;
        txt4 = Resources.Load("Cell_func_another", typeof(TextAsset)) as TextAsset;
        txt5 = Resources.Load("Cells_ini", typeof(TextAsset)) as TextAsset;
        txt6 = Resources.Load("Cells_another_ini", typeof(TextAsset)) as TextAsset;

        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        string TextLines2 = textasset2.text;
        string TextLines3 = txt3.text;
        string TextLines4 = txt4.text;
        string TextLines5 = txt5.text;
        string TextLines6 = txt6.text;

        //Splitで一行づつを代入した1次配列を作成
        rule_raw = TextLines.Split('\n'); //
        rule_raw2 = TextLines2.Split('\n');
        states_raw = TextLines3.Split('\n');
        states_raw2 = TextLines4.Split('\n');
        cells_raw = TextLines5.Split('\n');
        cells_raw2 = TextLines6.Split('\n');


        //行数と列数を取得
        columnLength = rule_raw[0].Split(',').Length;
        rowLength = rule_raw.Length;
        columnLength2 = rule_raw2[0].Split(',').Length;
        rowLength2 = rule_raw2.Length;
        func_length = states_raw[0].Split(',').Length;
        states_length = states_raw.Length;
        func_length2 = states_raw2[0].Split(',').Length;
        states_length2 = states_raw2.Length;
        cells_column = cells_raw[0].Split(',').Length;
        cells_row = cells_raw.Length;
        cells_column2 = cells_raw2[0].Split(',').Length;
        cells_row2 = cells_raw2.Length;

        //2次配列を定義
        rule_parameters = new string[rowLength, columnLength];
        rule_parameters2 = new string[rowLength2, columnLength2];
        states_parameters = new string[states_length, func_length];
        states_parameters2 = new string[states_length2, func_length2];
        cells_ini = new string[cells_row, cells_column];
        cells_another_ini = new string[cells_row2, cells_column2];

        for (int i = 0; i < rowLength; i++)
        {

            string[] tempWords = rule_raw[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++)
            {
                rule_parameters[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }

        for (int i = 0; i < rowLength2; i++)
        {

            string[] tempWords = rule_raw2[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength2; n++)
            {
                rule_parameters2[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }

        for (int i = 0; i < states_length; i++)
        {

            string[] tempWords = states_raw[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < func_length; n++)
            {
                states_parameters[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }

        for (int i = 0; i < states_length2; i++)
        {

            string[] tempWords = states_raw2[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < func_length2; n++)
            {
                states_parameters2[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }

        for(int i = 0; i < cells_column; i++)
        {
            for(int j = 0; j < cells_row; j++)
            {

            }
        }
    }
}

