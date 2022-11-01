using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckExcute : MonoSingleton<CheckExcute>
{
	class Condition
	{
		public float fCheckSec = 0f;
		public float fCheckSecCurr = 0f;
		public float fMaxWwait = 0f;
		public System.Func<bool> On_Condition;
		public System.Action On_Callback;
		public object tag ;
	}

	List<Condition> m_RemoveCond = null;
	List<Condition> m_ConditionList = new List<Condition>();

	public static void Subscribe( System.Func<bool> condition, System.Action callback, float fCheckSec = 0f, float fMaxWait = -1, object tag = null )
	{
		if(condition())
        {
			callback();
			return;
        }
		Condition cond = new Condition();
		cond.On_Condition = condition;
		cond.On_Callback = callback;
		cond.fCheckSec = fCheckSec;
		cond.fCheckSecCurr = 0f;
		cond.fMaxWwait = fMaxWait;
		cond.tag = tag;

		Get().m_ConditionList.Add( cond );
	}

	public static void RemoveByTag(object tag)
	{
		if (Get() == null ||  Get().m_ConditionList.Count == 0)
			return;

		for (int i = 0; i < Get().m_ConditionList.Count; i++)
		{
			Condition cond = Get().m_ConditionList[i];
			if(cond.tag == tag)
			{
				Get().m_ConditionList.RemoveAt(i);
				return;
			}
		}
	}

		public void Update()
	{
		if( m_ConditionList.Count == 0 )
			return;

		for( int i = 0; i < m_ConditionList.Count; i++ )
		{
			Condition cond = m_ConditionList[i];

			bool isConditionSuccess = false;
			if( cond.fMaxWwait > 0 )
			{
				cond.fMaxWwait -= Time.deltaTime;
				if( cond.fMaxWwait <= 0)
					isConditionSuccess = true;
			}

			if( isConditionSuccess == false && cond.fCheckSecCurr > 0 )
			{
				cond.fCheckSecCurr -= Time.deltaTime;
				continue;
			}
			else
				cond.fCheckSecCurr = cond.fCheckSec;
			
			bool isRemoveCondition = false;
			try
			{
				if( isConditionSuccess == false && 
					cond.On_Condition != null )
				{
					isConditionSuccess = cond.On_Condition();
				}
			}
			catch( System.Exception e )
			{
				LMJ.LogError( e );
				isRemoveCondition = true;
			}

			if( isConditionSuccess == true )
			{
				try
				{
					if( cond.On_Callback != null )
						cond.On_Callback();
				}
				catch( System.Exception e )
				{
					LMJ.LogError( e );
				}

				isRemoveCondition = true;
			}

			if( isRemoveCondition == true )
			{
				if( m_RemoveCond == null )
					m_RemoveCond = new List<Condition>();

				m_RemoveCond.Add( cond );
			}
		}

		if( m_RemoveCond != null && m_RemoveCond.Count != 0 )
		{
			for( int i = 0; i < m_RemoveCond.Count; i++ )
			{
				Condition condition = m_RemoveCond[i];
				m_ConditionList.Remove( condition );
			}

			m_RemoveCond.Clear();
			m_RemoveCond = null;
		}
	}

}