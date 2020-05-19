
document.addEventListener("DOMContentLoaded", function () {
    var scrollbar = document.body.clientWidth - window.innerWidth + 'px';
    console.log(scrollbar);
    document.querySelector('[href="#open-modal-add-room"]').addEventListener('click', function () {
        document.body.style.overflow = 'hidden';
        document.querySelector('#open-modal-add-room').style.marginLeft = scrollbar;
    });
    document.querySelector('[href="#close-modal-add-room"]').addEventListener('click', function () {
        document.body.style.overflow = 'visible';
        document.querySelector('#open-modal-add-room').style.marginLeft = '0px';
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var scrollbar = document.body.clientWidth - window.innerWidth + 'px';
    console.log(scrollbar);
    document.querySelector('[href="#open-modal-add-companion"]').addEventListener('click', function () {
        document.body.style.overflow = 'hidden';
        document.querySelector('#open-modal-add-companion').style.marginLeft = scrollbar;
    });
    document.querySelector('[href="#close-modal-add-companion"]').addEventListener('click', function () {
        document.body.style.overflow = 'visible';
        document.querySelector('#open-modal-add-companion').style.marginLeft = '0px';
    });
});

$('#messages-search').keyup(function () {
    var regexp = /currentRoomId=([^&]+)/i;
    var GetValue = '';
    if (!!regexp.exec(document.location.search)) {
        GetValue = regexp.exec(document.location.search)[0].split('=')[1];
        $.ajax({
            url: '/Home/MessagesSearch',
            method: 'POST',
            data: { 'searchKey': $('#messages-search').val(), 'currentRoomId': GetValue },
            dataType: 'html'
        }).done(function (html) {
            $('#place-for-messages').replaceWith(html);
        });
    }
        
});