$.ajax({
    url: "https://localhost:7112/mangenews/getNews",
    type: "get",
    contentType: "application/json",
    success: function (result, status, xhr) {
        console.log(result);
        $.each(result, function (index, value) {
            //$("tbody").append($("<tr>"));
            //appendElement = $("tbody tr").last();
            //appendElement.append($("<td>").html(value["id"]));
            //appendElement.append($("<td>").html(value["name"]));
            //appendElement.append($("<td>").html(value["startLocation"]));
            //appendElement.append($("<td>").html(value["endLocation"]));
            //appendElement.append($("<td>").html("<a href=\"UpdateReservation.html?id=" + value["id"] + "\"><img src=\"icon/edit.png\" /></a>"
            //        appendElement.append($("<td>").html("<img class=\"delete\" src=\"icon/close.png\" />"));
        });
    },
    error: function (xhr, status, error) {
        console.log(xhr)
    }
});