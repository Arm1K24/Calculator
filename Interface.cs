using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] private List<Button> _buttonUi;
    [SerializeField] private Logic _logic;

    private string _regular = "+÷✕-";

    public Text answerField; 
    public Text inputField;
    
    //Стартовая функция, добавляем ко всем кнопкам в UI функцию печати DataEntry()
    void Start()
    {
        foreach( Button _buttonUiItem in _buttonUi)
        {
            _buttonUiItem.onClick.AddListener(delegate { DataEntry(_buttonUiItem.GetComponentInChildren<Text>().text); });
        }

    }

    //Функция печати символов только для цифр
    public void DataEntry(string data)
    {
        if (LimitSymbol())
            return;
        if (inputField.text.Length > 0)
            if (inputField.text[inputField.text.Length - 1] == ')')
                inputField.text += "✕";
        inputField.text += data;
    }

    //Удаление последнего символа
    public void DataRemove()
    {
        if (inputField.text.Length > 0)
            inputField.text = inputField.text.Remove(inputField.text.Length-1, 1);
    }

    //Очистка поля ввода
    public void DataClear()
    {
        inputField.text = "";
    }

    /*
    Метод вставки операционных символов в поле, 
    перед этим проверяет не является ли последний символ уже символом операции, 
    для проверки использует переменную _regular
    */
    public void Operation(string _symbol)
    {
        //Если больше 20 символов, то больше не позволяем писать
        if (LimitSymbol())
            return;


        if (inputField.text.Length > 0)
            foreach (char _r in _regular)
            {
                if (inputField.text[inputField.text.Length - 1] == _r)
                {
                    return;
                }
            }
        inputField.text += _symbol;

    }

    //Метод вставки точки, можно было поебаться с абстракцией или нельзя и я долбаеб
    public void Comma()
    {
        //Если больше 20 символов, то больше не позволяем писать
        if (LimitSymbol())
            return;


        if (inputField.text.Length > 0)
        {
            foreach (char _r in _regular)
            {
                if (inputField.text[inputField.text.Length - 1] == _r)
                {
                    return;
                }
            }
            if (inputField.text[inputField.text.Length - 1] == '.')
            {
                return;
            }
        }
        inputField.text += ".";
    }

    //Метод открытия скобок, такой же длинный как мой хуй)
    //При открытии проверяет можно ли заебашить умножение в перед скобкой
    public void OpeningParenthesis()
    {
        //Если больше 20 символов, то больше не позволяем писать
        if (LimitSymbol())
            return;

        bool _haveSymbol = false;

        if (inputField.text.Length > 0)
        {
            foreach (char _r in _regular)
            {
                if (inputField.text[inputField.text.Length - 1] == _r)
                {
                    _haveSymbol = true;
                }
            }
            if (inputField.text[inputField.text.Length - 1] == '-')
            {
                _haveSymbol = true;
            }
            if (inputField.text[inputField.text.Length - 1] == '(')
            {
                _haveSymbol = true;
            }

            if (!_haveSymbol)
            {
                inputField.text += "✕";
            }
        }
        
        inputField.text += "(";
    }

    //Закрытие скобок, разрешает закрыть, если есть открытые скобки
    public void ClosingParenthesis()
    {
        int countParenthesis = 0;
        foreach(char _s in inputField.text)
        {
            if (_s == '(')
                countParenthesis++;
            if (_s == ')')
                countParenthesis--;
        }
        if (countParenthesis > 0)
            inputField.text += ")";
    }

    //Равно))
    public void Equal(string _answer)
    {
        answerField.text = _answer; //Выводит равно в поле ответа
    }

    


    //Лимит символов изменять здесь
    private bool LimitSymbol()
    {
        if (inputField.text.Length > 20)
            return true;
        return false;
    }
}


