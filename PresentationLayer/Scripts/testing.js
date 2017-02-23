
$(document).ready(function () {
    $("#Slots").change(function () {
        if ($("#Slots").val()) {
            var option = $("#Slots").val();
            $("#Slot").val(option);
        }
    });
    $("#Date").change(function () {
        if ($("#Date").val()) {
            var date = $("#Date").val();
            $("#Slot").val(option);
        }
    });
});

//$(document).ready(function () {
//    $("#Slots").change(function () {
//        var option = $("#Slots").val();
//        $.ajax({
//            data: JSON.stringify({ Slots: $("#Slots").val() }),
//            type: "post",
//            success: function (setSlot) {
//                var option = $("#Slots").val();
//                $("#Slot").val(option);
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                $('#Slots').text("Error encountered while selecting slot.");
//            }
//        });
//    });
//});



//$(document).ready(function () {
    //    $("#State").prop("disabled", true);
    //    $("#Country").change(function () {
    //        if ($("#Country").val() != "Select") {
    //            var CountryOptions = {};
    //            CountryOptions.url = "/Sample/states";
    //            CountryOptions.type = "POST";
    //            CountryOptions.data = JSON.stringify({ Country: $("#Country").val() });
    //            CountryOptions.datatype = "json";
    //            CountryOptions.contentType = "application/json";
    //            CountryOptions.success = function (StatesList) {
    //                $("#State").empty();
    //                for (var i = 0; i < StatesList.length; i++) {
    //                    $("#State").append("<option>" + StatesList[i] + "</option>");
    //                }
    //                $("#State").prop("disabled", false);
    //            };
    //            CountryOptions.error = function () { alert("Error in Getting States!!"); };
    //            $.ajax(CountryOptions);
    //        }
    //        else {
    //            $("#State").empty();
    //            $("#State").prop("disabled", true);
    //        }
    //    });
    //});