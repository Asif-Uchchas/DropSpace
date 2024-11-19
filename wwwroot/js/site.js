

$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();
    connection.on("LoadNotifications", function () {
        LoadNotification();
    })

    LoadNotification();

    function LoadNotification() {
        var tr = '';

        $.ajax({
            url: '/Home/GetNotifications',
            method: 'GET',
            success: (result) => {
                var appLength = result.length;
                $.each(result, (k, v) => {
                    tr = tr + `<a class="dropdown-item d-flex align-items-center" href="javascript:void(0)">
                        <div class="mr-3">
                            <div class="icon-circle bg-warning">
                                <i class="fas fa-exclamation-triangle text-white"></i>
                            </div>
                        </div>
                        <div>
                            <div class="small text-gray-500">${v.gdDate}</div>
                            <span>${v.trackingNumber},</span>
                            <span>${v.gdObject}</span>
                        </div>
                    </a>`
                })
                $('#notifyCnt').html(appLength);
                $('#tableBody').html(tr);
            },
            error: (error) => {
                console.log(error);
            }
        })
    }

})
