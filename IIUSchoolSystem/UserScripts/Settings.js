$(document).ready(function () {
    $('#CreateOrUpdateSetting').click(function () {
        var vl = $('#settingForm').valid();
        if (vl) {
            CreateOrUpdateSettings();
        }
    });
    $('#createSetting').click(function () {
        $('#createAccount').slideToggle(500);
        ResetFields();
        $('#headingSetting').html("");
        $('#headingSetting').html('<i class="icon-reorder"></i>New Setting');
    });
    $('#cancelSettingCreation').click(function () {
        $("#settingForm input[type='text']").removeClass("error");
        $('#createAccount').slideToggle(500);
        $("#Id").val(0);
        ResetFields();
        $('#headingSetting').html("");
        $('#headingSetting').html('<i class="icon-reorder"></i>New Setting');
    });
});

function EditSettings(id) {
    var request = "{'id':'" + id + "'}";
   
    $.ajax({
        type: "POST",
        url: GetSiteRoot() + "GetSettings",
        data: request,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data;
            if (msg.hasOwnProperty("d")) {
                data = msg.d;
            }
            else
                data = msg;
            if (data.success) {
                FillEditForm(data.Setting);
            }
            else {
                Notify(data.message, 'error')
            }
        },
        error: OnError
    });
}

function FillEditForm(data) {

    $("#settingForm input[type='text']").removeClass("error");

    $('#hdnId').val(data.Id);
    $('#SettingName').val(data.SettingName);
    $('#Value').val(data.Value);
    $('#Description').val(data.Description);

    $('#headingSetting').html("");
    $('#headingSetting').html('<i class="icon-reorder"></i>Update Setting');

    $('#createAccount').slideToggle(500);
}

function CreateOrUpdateSettings() {

    var request = { Id: $("#hdnId").val(), SettingName: $('#SettingName').val(), Value: $('#Value').val(), Description: $('#Description').val() };

    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'CreateOrUpdateSettings',
        data: JSON.stringify({ 'model': request }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data;
            if (msg.hasOwnProperty("d")) {
                data = msg.d;
            }
            else
                data = msg;
            if (data.success) {
                Notify(data.message, 'success');
                $('#createAccount').slideToggle(500);
                ResetFields();
                RefreshGrid("settings-grid");
            }
            else {
                Notify(data.message, 'error');
            }
            $("#Id").val(0);
        },
        error: OnError
    });
    RefreshGrid("settings-grid");
}

function onDataBinding(e) {
}

function ConfirmDelete(id) {
    var messageTitle = "<div class='widget-header'><h4 class='smaller'><i class='fa fa-warning red'></i> Delete Confirmation</h4></div>";

    $("#dialog-confirm").removeClass('hide').dialog({
        resizable: false,
        width: '364',
        modal: true,
        buttons: [
            {
                html: "<i class='fa fa-trash-o bigger-110'></i>&nbsp; Delete",
                "class": "btn btn-danger btn-primary",
                click: function () {
                    DeleteSetting(id);
                    $(this).dialog("close");
                }
            }
            ,
            {
                html: "<i class='fa fa-times bigger-110'></i>&nbsp; Cancel",
                "class": "btn btn-primary",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
            show: 'blind',
            hide: 'fold',
            closeOnEscape: true,
    }).dialog('widget').find('.ui-dialog-title').html(messageTitle);
}

function DeleteSetting(id) {
 
    var request = "{'Id':'" + id + "'}";
    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'Delete',
        data: request,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data;
            if (msg.hasOwnProperty("d")) {
                data = msg.d;
            }
            else
                data = msg;
            if (data.success) {
                Notify(data.message, 'success');
                RefreshGrid("settings-grid");
            } else {
                Notify(data.message, 'error');
            }
        },
        error: OnError
    });
}