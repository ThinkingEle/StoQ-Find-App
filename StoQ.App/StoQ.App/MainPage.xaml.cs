using StoQ.App.Models;
using StoQ.App.Services;

namespace StoQ.App;

public partial class MainPage : ContentPage
{
    private readonly ApiService _apiService = new();
    private string _currentParentId;

    // 기본 생성자 (Root 조회용)
    public MainPage() : this(null) { }

    // 계층 이동용 생성자
    public MainPage(string parentId)
    {
        InitializeComponent();
        _currentParentId = parentId;

        // UI 타이틀 설정
        Title = string.IsNullOrEmpty(parentId) ? "StoQ - 홈" : "내용물 확인";

        // 아이템 클릭 이벤트 연결
        NodesListView.SelectionChanged += OnNodeSelected;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadNodes();
    }

    private async Task LoadNodes()
    {
        try
        {
            var nodes = await _apiService.GetNodesAsync(_currentParentId);
            NodesListView.ItemsSource = nodes;
        }
        catch (Exception ex)
        {
            await DisplayAlert("오류", "데이터를 가져오지 못했습니다.", "확인");
        }
    }

    private async void OnNodeSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is NodeModel selectedNode)
        {
            // 선택 해제 (다시 돌아왔을 때를 위해)
            ((CollectionView)sender).SelectedItem = null;

            // 아이템 타입이 'ITEM'이더라도 스토리지가 될 수 있으므로 이동 허용
            // (사용자 기획서의 "아이템이 스토리지 역할을 할 수 있다" 반영)
            await Navigation.PushAsync(new MainPage(selectedNode.Id));
        }
    }

    private async void OnScanClicked(object sender, EventArgs e)
    {
        // QR 스캔 페이지로 이동 (나중에 구현)
        await DisplayAlert("알림", "QR 스캔 기능을 준비 중입니다.", "확인");
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        // 추가하기 기능 (나중에 구현)
        await DisplayAlert("알림", "추가하기 기능을 준비 중입니다.", "확인");
    }
}