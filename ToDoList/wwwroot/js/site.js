$(function () {
    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});

$("#dataProvider").change(function (e) {
    document.cookie = `DataProvider=${e.target.value}`;
    document.location.reload();
});