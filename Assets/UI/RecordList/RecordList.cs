using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class RecordList : MonoBehaviour
{
    [SerializeField] private SceneManagerSO sceneManager;
    [SerializeField] private TimerSO timer;

    private UIDocument UI;
    private MultiColumnListView mc;
    private readonly CancellationTokenSource cts = new();
    private float lastMeasuredWidth = -1f;

    private List<ApiClient.Record> data = new() { };

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        Button playButton = UI.rootVisualElement.Query<Button>("play");
        playButton.clicked += () =>
        {
            StartCoroutine(sceneManager.LoadLevel(1));
            timer.Reset();
        };

        mc = UI.rootVisualElement.Query<MultiColumnListView>("list");
        mc.itemsSource = data;

        var col0 = new Column
        {
            title = "Username",
            makeCell = () =>
            {
                var el = new Label { name = "cellLabel" };
                el.style.unityTextAlign = TextAnchor.MiddleLeft;
                el.style.paddingLeft = 4;
                return el;
            },
            bindCell = (ve, rowIndex) =>
            {
                var item = data[rowIndex];
                ve.Q<Label>("cellLabel").text = item.username;
            }
        };

        var col1 = new Column
        {
            title = "Time",
            makeCell = () =>
            {
                var el = new Label { name = "cellLabel" };
                el.style.unityTextAlign = TextAnchor.MiddleLeft;
                el.style.paddingLeft = 4;
                return el;
            },
            bindCell = (ve, rowIndex) =>
            {
                var item = data[rowIndex];
                ve.Q<Label>("cellLabel").text = item.time.ToString();
            }
        };

        var col2 = new Column
        {
            title = "Created At",
            makeCell = () =>
            {
                var el = new Label { name = "cellLabel" };
                el.style.unityTextAlign = TextAnchor.MiddleLeft;
                el.style.paddingLeft = 4;
                return el;
            },
            bindCell = (ve, rowIndex) =>
            {
                var item = data[rowIndex];
                string readable;
                if (DateTimeOffset.TryParse(item.createdAt, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dto))
                    readable = dto.ToLocalTime().ToString("dd MMM yyyy HH:mm");
                else
                    readable = item.createdAt;

                ve.Q<Label>("cellLabel").text = readable;
            }
        };

        mc.columns.Add(col0);
        mc.columns.Add(col1);
        mc.columns.Add(col2);

        mc.RegisterCallback<GeometryChangedEvent>(evt => UpdateColumnWidths());
        UI.rootVisualElement.RegisterCallback<GeometryChangedEvent>(evt => UpdateColumnWidths());

        mc.RefreshItems();

        _ = UpdateDataPeriodicalAsync(cts.Token);
    }

    private async Task UpdateDataPeriodicalAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var newData = await ApiClient.GetAllRecordsAsync(cts.Token);
                data.Clear();
                data.AddRange(newData);
                mc.RefreshItems();
            }
            catch (ApiClient.ApiException ex)
            {
                Debug.LogError($"API error ({ex.StatusCode}): {ex.Message}");
                return;
            }

            await Task.Delay(1000, ct);
        }
    }

    private void UpdateColumnWidths()
    {
        float available = mc.contentRect.width;
        if (available <= 0f)
        {
            available = mc.layout.width;
            if (available <= 0f) return;
        }

        if (Mathf.Approximately(lastMeasuredWidth, available)) return;
        lastMeasuredWidth = available;

        int count = mc.columns.Count;
        if (count == 0) return;

        float each = available / count;
        for (int i = 0; i < count; i++)
            mc.columns[i].width = each;

        mc.Rebuild();
    }
}