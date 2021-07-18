$(document).ready(function () {

    $("#imageBrowes").change(function () {

        var File = this.files;

        if (File && File[0]) {
            ReadImage(File[0]);
        }

    })


})


var ReadImage = function (file) {

    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {

        image.src = _file.target.result;
        image.onload = function () {

        
            $("#targetImg").attr('src', _file.target.result);
            $("#targetImg").attr("style", "height:150px;width: 150px;");
           $("#imgPreview").show();

        }

    }

}