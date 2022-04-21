using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;


public class Logic : MonoBehaviour
{
    [SerializeField] private Interface _interface;

    CultureInfo culture = new CultureInfo("en-US");
    
    //Функция к которому обращается прога, которая после через интерфейс выводит ответ
    public void EqualMath()
    {
        _interface.Equal(EMath(RePast(_interface.inputField.text)));

    }

    /*
    Эта функция доходит до последнего выражения в скобках, отправляет ее на решение, 
    после поднимаясь на скобку выше постепенно отправляет на решение и заменяет ответ на все выражение в скобках
    в итоге остается все выражение без скобках(уже решено все что в скобках) 
    */
    private string RePast(string _expression)
    {
        
        string _s = Split(_expression);

        if(_s == null){
            return _expression;
        }
        

        while(true){
            if(CountSymbol(_s, '(') == 0)
                break;
            
            _s = Split(_s);
            
            if(_s == null){
                return _expression;
            }

        }

        _expression = _expression.Replace("(" + _s + ")", EMath(_s));

        if(CountSymbol(_expression, '(')!=0){
            _expression = RePast(_expression);
        }
        
        return _expression;
    }
    
    /*
    Вырезает выражение по скобках, то есть выражение 3*(5+(2-3)) выдаст ответ 5+(2-3)
    а RePast с его помощью доходит до последнего в скобке
    */
    private string Split(string _expression)
    {
        int _startSplit = -1;
        int _endSplit = -1;
        int countParenthesis = 0;

        for(int i = 0; i<_expression.Length; i++){

            if(_expression[i] == '('){
                if(countParenthesis == 0){
                    _startSplit = i+1;
                    countParenthesis++;
                }else{
                    countParenthesis++;
                }
            }

            if(_expression[i] == ')')
            {
                if(countParenthesis == 1)
                {
                    _endSplit = i;
                    countParenthesis--;
                }else
                {
                    countParenthesis--;
                }
            }

        }

        if(_startSplit == -1)
        {
            return null;
        }

        if(_endSplit == -1)
        {
            _endSplit = _expression.Length-1;
        }

        return _expression.Substring(_startSplit, _endSplit - _startSplit);
    }

    /*
    Своя функция подсчета количество определенных символов в строке
    в основном использую чтобы узнать есть ли еще скобки в выражении или уже мы дошли до последнего и нужно решать ее
    */
    private int CountSymbol(string _expression, char _symbol)
    {
        int count = 0;
        if(_expression == null)
            return count;
        foreach(char _e in _expression){
            if(_e == _symbol)
                count++;
        }
        return count;
    }

    //Тут настраиваем порядок выполнения выражения используя методы в классе
    private string EMath(string _expression)
    {
        
        // Порядок выполнения операции должно сохраняться
        _expression = Mul(_expression);
        _expression = Share(_expression);
        _expression = Plus(_expression);
        _expression = Minus(_expression);
        return _expression;
    }

    //Пошел жестким говнокод, простите меня

    /*
        При добавлении нового функционала, необходимо добавить его здесь и узнать его порядковый номер
        Конченный Split, хули он не возвращает массив с наличием того параметра с помощью которого мы сплитим, сука
    */
    private string[] SplitSpecial(string _expression)
    {
        char[] _separators = new char[] {'✕', '÷', '+', '-' };

        string _separatorsReplace = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // Порядковый номер операции, 
        int count = 0;
        foreach(char _s in _separators)
        {
            string _sString = System.Convert.ToString(_s);
            _expression = _expression.Replace( _sString, _sString + _separatorsReplace[count] + _sString );
            count++;
        }
        string[] _arrayExpression = _expression.Split(_separators);

        return _arrayExpression;
    }

    //Умножение
    private string Mul(string _expression)
    {
        
        string[] _arrayExpression = SplitSpecial(_expression);
        
        for(int i= 0; i<_arrayExpression.Length; i++)
        {
            if(_arrayExpression[i] == "A")
            {
                _expression = _expression.Replace(_arrayExpression[i-1]+"✕"+_arrayExpression[i+1], System.Convert.ToString(System.Convert.ToSingle(_arrayExpression[i-1], culture)*System.Convert.ToSingle(_arrayExpression[i+1], culture)));
            }
        }
        return _expression;
    }

    //Деление
    private string Share(string _expression)
    {
        
        string[] _arrayExpression = SplitSpecial(_expression);
        
        for(int i = 0; i<_arrayExpression.Length; i++)
        {
            if(_arrayExpression[i] == "B")
            {
                _expression = _expression.Replace(_arrayExpression[i-1]+"÷"+_arrayExpression[i+1], System.Convert.ToString(System.Convert.ToSingle(_arrayExpression[i-1], culture) / System.Convert.ToSingle(_arrayExpression[i+1], culture)));
            }
        }
        return _expression;
    }
    //ПЛюс)
    private string Plus(string _expression)
    {
        
        string[] _arrayExpression = SplitSpecial(_expression);
        
        for(int i= 0; i<_arrayExpression.Length; i++)
        {
            if(_arrayExpression[i] == "C")
            {
                _expression = _expression.Replace(_arrayExpression[i-1]+"+"+_arrayExpression[i+1], System.Convert.ToString(System.Convert.ToSingle(_arrayExpression[i-1], culture) + System.Convert.ToSingle(_arrayExpression[i+1], culture)));
            }
        }
        return _expression;
    }
    //Минус
    private string Minus(string _expression)
    {
        
        string[] _arrayExpression = SplitSpecial(_expression);
        
        for(int i= 0; i<_arrayExpression.Length; i++)
        {
            if(_arrayExpression[i] == "D")
            {
                _expression = _expression.Replace(_arrayExpression[i-1]+"-"+_arrayExpression[i+1], System.Convert.ToString(System.Convert.ToSingle(_arrayExpression[i-1], culture) - System.Convert.ToSingle(_arrayExpression[i+1], culture)));
            }
        }
        return _expression;
    }

}


