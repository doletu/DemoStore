function CofirmDelete(Id) {
    $("#myModal").modal('show');
    $("#Id").val(Id);

}

function DeleteAccount() {

    $("#loaderDiv").show();
    var Id = $("#Id").val();
    $.ajax({

        type: "POST",
        url: "/Home/Delete",
        data: { UserId: Id },
        success: function (result) {
            $("#loaderDiv").hide();
            $("#myModal").modal("hide");
            $("#row_" + Id).remove();

        }

    })

}
 