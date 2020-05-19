
$('#change-user-info').on('click', function () {
    $('#change-user-info-form').removeAttr('hidden');
    $('#change-user-info').attr('hidden', true);
});

$('#hide-form').on('click', function () {
    $('#change-user-info-form').attr('hidden', true);
    $('#change-user-info').removeAttr('hidden');
})

var inputs = document.querySelectorAll('#user-picture-input');
Array.prototype.forEach.call(inputs, function (input) {
    var label = input.nextElementSibling,
        labelVal = label.innerHTML;
    input.addEventListener('change', function (e) {
        var fileName = '';
        if (this.files && this.files.length > 1)
            fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
        else
            fileName = e.target.value.split('\\').pop();

        if (fileName) {
            label.querySelector('span').innerHTML = fileName;
            var data = new FormData();
            data.append("avatar", this.files[0]);

            $.ajax({
                url: '/Account/ChangeAvatar',
                type: 'POST',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                encType: 'multipart/form-data',
                success: function (img) {
                    $('#user-photo img').replaceWith(img);
                },
                error: function () {
                    alert('Произошла ошибка при загрузке фотографии');
                }
            });
        }
        else
            label.innerHTML = labelVal;
    });
});



