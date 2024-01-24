using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// An index handler stores a simple int value used as an index. by modifying the value through the handler, all of the required logic for the
/// index will not crowd the original script.
/// </summary>
public class IndexHandler
{
    public int testindex = 0;
    public int index { get { return m_index;}}
    public int indexMaximumValue { get{return m_indexMaximumValue; }}

    [SerializeField] private int m_index = 0;
    [SerializeField] private int m_indexMaximumValue = 0;


    public void SetMaxIndexValue(int newMaxValue = 0)
    {
        m_indexMaximumValue = newMaxValue;
    }

    public void NextIndex()
    {
        m_index = (index + 1 > indexMaximumValue) ?  0 : index + 1 ;
    }

    public void PreviousIndex()
    {
        m_index = (index - 1 < 0 ) ? indexMaximumValue : index - 1;
    }

    public void FirstIndex()
    {

    }

    /// <summary>
    /// Attempts to set the current index to the given value. If value exceeds the list limit, it will cycle through the list.
    /// </summary>
    /// <param name="newIndex"></param>
    /// <returns></returns>
    public bool SetcurrentIndex(int newIndex = 0)
    {
        //Index is possible
        if(newIndex <= m_indexMaximumValue && newIndex > -1)
        {
            m_index = newIndex;
            return true;
        }

        //Index overflows
       else if(newIndex > m_indexMaximumValue)
        {
            int overflowValue = newIndex % m_indexMaximumValue;
            m_index = overflowValue;
            return true;
        }

        // give up, just give up now, sort yourself out 
        else
        {
            //return to default value
            m_index = 0;
            return false;
        }
    }


}
