using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class LifeUI : MonoBehaviour
{
    public int lifeperImage = 10;
    public int width = 300;                         // GridLayoutGroup�� width�� : lifeperImage * gridLayoutGroup.cellSize.x�� ���
    public GameObject lifePrefab;
    public UnityEvent onChanged;

    [SerializeField] private List<GameObject> m_lifePrefabList = new List<GameObject>();
    private GridLayoutGroup m_gridLayoutGroup;
    private int m_currentPrefabCount;
    private float m_currentRemain;


    private void Awake()
    {
        m_gridLayoutGroup = GetComponent<GridLayoutGroup>();

        onChanged.RemoveAllListeners();
        onChanged.AddListener(OnUpdate);
    }

    private void OnDestroy()
    {
        onChanged.RemoveAllListeners();
    }

    private void Start()
    {
        // ���� lifePrefab�� ��� ����
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int prefabCount = 0;                                    // life prefab�� ����
        float remain = 0.0f;                                    // ������ life prefab�� fill amount
        CalculatePrefabCount(ref prefabCount, ref remain);

        InstantiateLifePrefab(prefabCount, remain);
    }

    private void Update()
    {
        OnUpdate();
    }

    private void CalculatePrefabCount(ref int prefabCount, ref float remain)
    {
        // Player�� life�� lifeperImage���� ������� lifePrefab�� �����Ͽ� m_lifeList�� �߰�
        float playerLife = GameManager.Instance.Player.playerLife.CurrentLifes;
        prefabCount = Mathf.Clamp(Mathf.CeilToInt(playerLife / lifeperImage), 0, 10000);    // 10000�� ������ ū ��
        m_currentPrefabCount = prefabCount;

        // fill amount�� ����ϱ� ���� ���� life�� ����
        remain = Mathf.Clamp(playerLife - prefabCount * lifeperImage, 0, lifeperImage);
        m_currentRemain = remain;

        // Debug.Log("current life : " + playerLife + " / prefabCount : " + prefabCount + " / remain : " + remain);
    }

    private void InstantiateLifePrefab(int prefabCount, float remain)
    {
        // prefabCount��ŭ Life prefab ����
        for (int i = m_lifePrefabList.Count; i < prefabCount; i++)
        {
            GameObject life = Instantiate(lifePrefab, transform);
            m_lifePrefabList.Add(life);
        }

        // remain�� �ִٸ� fill amount�� ����Ͽ� ������ Life prefab ����
        if (remain > 0)
        {
            GameObject life = Instantiate(lifePrefab, transform);
            Image image = life.GetComponent<Image>();
            image.fillAmount = remain;
            m_lifePrefabList.Add(life);
        }

        // GridLayoutGroup�� width���� ���, Size ����
        int denom = (prefabCount + (remain > 0 ? 1 : 0));
        m_gridLayoutGroup.cellSize = new Vector2(width / denom, m_gridLayoutGroup.cellSize.y);
    }

    private void OnUpdate()
    {
        // current life ���� ����
        int prefabCount_prev = m_currentPrefabCount;
        float remain_prev = m_currentRemain;

        // current life ���� ����
        int prefabCount = 0;
        float remain = 0.0f;
        CalculatePrefabCount(ref prefabCount, ref remain);
        // Debug.Log("prev : " + prefabCount_prev + " / current : " + prefabCount);

        // prev�� current�� ���Ͽ� lifePrefab�� �߰��ϰų� ����
        // prev < current : lifePrefab �߰�
        if (prefabCount_prev < prefabCount)
        {
            for (int i = prefabCount_prev; i < prefabCount; i++)
            {
                GameObject life = Instantiate(lifePrefab, transform);
                m_lifePrefabList.Add(life);
            }
        }

        // prev > current : lifePrefab ����
        else if (prefabCount_prev > prefabCount)
        {
            for (int i = prefabCount_prev - 1; i >= prefabCount; i--)
            {
                DestroyLifePrefab(m_lifePrefabList[i]);
            }
        }

        // prev == current : ������ lifePrefab�� fill amount�� ����
        if (remain_prev != remain)
        {
            Image image = m_lifePrefabList[m_lifePrefabList.Count - 1].GetComponent<Image>();
            image.fillAmount = remain;
        }

    }

    private void DestroyLifePrefab(GameObject lifePrefab)
    {
        // lifePrefab�� Image�� Color�� red�� �ٲٰ�, FadeOut, ScaleUp�� �� Destroy
        Image image = lifePrefab.GetComponent<Image>();

        // DoTween�� ����Ͽ� lifePrefab�� Image�� Color�� red�� �ٲٰ�, FadeOut
        image.DOColor(Color.red, 0.5f);
        image.DOFade(0, 0.5f);

        // y������ ScaleUp
        image.transform.DOScaleY(3.0f, 0.5f).OnComplete(() => 
        {
            m_lifePrefabList.Remove(lifePrefab);
            Destroy(lifePrefab);
        });
    }
}
