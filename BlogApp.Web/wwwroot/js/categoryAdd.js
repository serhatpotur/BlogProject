$(document).ready(function () {
    $("#btnAdd").click(function (e) {
        e.preventDefault();

        var addUrl = app.Urls.categoryAddUrl;
        var redirectUrl = app.Urls.articleAddUrl;

        var addCategoryDto = {
            Name: $("input[id=categoryName]").val()
        };

        var jsonData = JSON.stringify(addCategoryDto);
        console.log(jsonData);

        $.ajax({
            url: addUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: jsonData,
            success: function (data) {
                setTimeout(function () {
                    window.location.href = redirectUrl;
                }, 1500);
            },
            error: function () {
                toastr.error("Bir Hata Oluþtur!", "Hata");
            }
        });
    })
})