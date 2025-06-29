using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
	[SerializeField]
	GameObject m_textWindow;

	[SerializeField]
	GameObject m_textObject;

	[SerializeField]
	GameObject m_statusObject;

	[SerializeField]
	TextMeshProUGUI m_unitText;

	[SerializeField]
	TextMeshProUGUI m_unitStatus;

	[SerializeField]
	GameObject m_gameController;

	[SerializeField]
	Image m_image;

	[SerializeField]
	GameObject[] m_textChanger;

	int count;
	Unit m_unit;

	private void Start()
	{
		DefText();
		count = 0;
	}

	public void OnInformation(Unit unit)
	{
		foreach(GameObject button in m_textChanger)
		{
			button.SetActive(true);
		}
		m_unit = unit;
		m_textWindow.SetActive(true);
		m_image.sprite = unit.Sprite;
		TextChange();
	}

	public void TextChange()
	{
		//countが奇数か偶数かによってボタンの持つ意味を変える
		count++;
		if (count % 2 == 0)
		{
			Status(m_unit);
			Debug.Log(count + " = 2?"); 
		}
		else
		{
			TextInfo(m_unit);
			Debug.Log(count + " = 1?"); 
		}
	}


	public void DefText()
	{
		foreach (GameObject button in m_textChanger)
		{
			button.SetActive(true);
		}
		m_statusObject.SetActive(true);
		m_textObject.SetActive(true);
		m_textWindow.SetActive(false);
		m_unit = null;
	}

	void Status(Unit unit)
	{
		count = 0;
		m_statusObject.SetActive(true);
		m_textObject.SetActive(false);
		m_unitStatus.text = " ユニットネーム : " + unit.Name + "\n" +
							" エネミーレベル : " + unit.FriendLevel + "\n" +
							" HP  : " + unit.HealthValue + "\n" +
							" ATK : " + unit.AttackValue + "\n" +
							" DEF : " + unit.DefenseValue + "\n" +
							" AGI : " + unit.Agility; 
	} 

	void TextInfo(Unit unit)
	{
		count = 1;
		m_statusObject.SetActive(false);
		m_textObject.SetActive(true);
		m_unitText.text = unit.Name + " : \n" + unit.Information;
	}
}
