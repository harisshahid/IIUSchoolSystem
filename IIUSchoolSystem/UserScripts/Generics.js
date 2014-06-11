$(document).ready(function () {
    // widget tools
    $('.tooltipClass').tooltip();
    jQuery('.widget .tools .icon-chevron-down').click(function () {
        var el = jQuery(this).parents(".widget").children(".widget-body");
        if (jQuery(this).hasClass("icon-chevron-down")) {
            jQuery(this).removeClass("icon-chevron-down").addClass("icon-chevron-up");
            el.slideUp(200);
        } else {
            jQuery(this).removeClass("icon-chevron-up").addClass("icon-chevron-down");
            el.slideDown(200);
        }
    });

    jQuery('.widget .tools .icon-remove').click(function () {
        jQuery(this).parent(".widget").parent().remove();
    });

    //$(".chosen-select").chosen();
  
});

function GetRemaingDays() {
    var current_date = new Date();
    var last_date_month = new Date(current_date.getFullYear(), current_date.getMonth() + 1, 1); //Last 1 means adding one more day

    return CalculateDays(current_date, last_date_month);
}

function CalculateDays(date1, date2) {

    // The number of milliseconds in one day
    var ONE_DAY = 1000 * 60 * 60 * 24;

    // Convert both dates to milliseconds
    var date1_ms = date1.getTime();
    var date2_ms = date2.getTime();

    // Calculate the difference in milliseconds
    var difference_ms = Math.abs(date1_ms - date2_ms);

    // Convert back to days and return
    return Math.round(difference_ms / ONE_DAY);
}

function GetTotalDaysInMonth() {
    var date = new Date();
    return new Date(date.getYear(), date.getMonth() + 1, 0).getDate();
}

function Notify(message, messageType) //message is the original message, messageType has values: success, warning, error values
{
    var parentContainer = '<div class="' + messageType + '"></div>';
    var messageTitle = "";
    var alert = '';

    if (messageType == "success")
    { messageTitle = 'Process completed successfully'; }

    if (messageType == "error")
    { messageTitle = 'An error occured while processing the request'; }

    if (messageType == "warning")
    { messageTitle = 'System generated a warning while processing'; }

    if (!message)
        message = 'No Message to Display.';

    messageTitle = "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='fa fa-check'></i> " + messageTitle + "</h4></div>";
    var alert = "<h2>" + message + "</h2>";

    $(parentContainer).html(alert).dialog({
        resizable: false,
        width: "435",
        modal: true,
        buttons: [
       {
           text: "Dismiss",
           "class": "btn btn-primary",
           click: function () {
               $(this).dialog("close");
           }
       }
        ],
        show: 'blind',
        hide: 'fold',
        closeOnEscape: true
    }).dialog('widget').find('.ui-dialog-title').html(messageTitle);
}

function OnError(msg) {
    var data;
    if (msg.hasOwnProperty("d")) {
        data = msg.d;
    }
    else
        data = msg;

    Notify(data.message, 'error');
}

function RefreshGrid(gridId) {
    var grid = $("#" + gridId).data("tGrid");
    grid.ajaxRequest();
}

function ResetFields() {
    $('.widget').find('input:text').not(':disabled').val('');
    $('.widget').find('textarea').val('');
    $('.widget').find('input:password').val('');
    $('.widget').find('input:hidden').val('0');
    $("select").removeAttr("readonly", "");
    $("body input[type='checkbox']").trigger('click', function () {
        $(this).prop("checked", false);
    });
    $("body").find("input[type='checkbox']").each(function () {
        $(this).prop("checked", false);
    });
}

$(document).ajaxStart(BlockUI).ajaxStop(UnBlockUI);

function BlockUI() {
    $('body').append('<div class="block-ui" style="text-align:center; z-index: 52031;position: absolute;margin-left: 50%;"><img src="../content/images/ajax-loader1.gif"), false)"></div>');
}

function UnBlockUI() {
    $('body').find('.block-ui').remove();
}

function CommunicationError() {
    Notify('An error occured while communicating with the server, please try again later.', 'error');
}

function onChange_SectionName(e) {
    var sectionName = $("#AjaxAutoComplete").data("tAutoComplete").value();
    if (sectionName.length > 0) {
        //  $('#CustomerId').val(sectionName);
    }
    else {

    }
}

function IsNumber(value) {
    var isnum = /^\d+$/.test(value);
    return isnum;
}

$('#main').on('click', function () {
    var $dropDiv = $('#dropDiv');// get position of the element we clicked on
    var offset = $(this).offset();

    // get width/height of click element
    var h = $(this).outerHeight();
    var w = $(this).outerWidth();

    // get width/height of drop element
    var dh = $dropDiv.outerHeight();
    var dw = $dropDiv.outerWidth();

    // determine middle position
    var initLeft = offset.left + ((w / 2) - (dw / 2));

    // animate drop
    $dropDiv.css({ left: initLeft, top: $(window).scrollTop() - dh, opacity: 0, display: 'block' })
        .animate({ left: initLeft, top: offset.top - dh, opacity: 1 }, 800, 'easeOutBounce');
});

function GetSiteRoot() {
    var rootPath = window.location.protocol + "//" + window.location.host + "/";
    if ((window.location.hostname == "localhost")) {
        var path = window.location.pathname;
        if (path.indexOf("/") == 0) {
            path = path.substring(1);
        }
        path = path.split("/", 1);
        if (path != "") {
            rootPath = rootPath + path + "/";
        }
    }
    else {
        var path = window.location.pathname;
        path = path.split("/");
        rootPath = rootPath + path[1] + "/" + path[2] + "/";
    }
	
	// var rootUrl  = window.location.href.split('/')[0] + "//" + window.location.href.split('/')[2] + "/" + window.location.href.split('/')[3];
       //return rootUrl;
    return rootPath;
}

$(function () {

    var path = window.location.pathname;
    path = path.split("/");
    var pageUrl = path[1] + "/" + path[2];

    switch (pageUrl) {
        case "Tenant/Index": ApplyClass("Tenant", 0, "");
            break;
        case "Tenant/TenantPayments": ApplyClass("Payments", 0, "");
            break;
        case "Prospects/Index": ApplyClass("Prospect", 0, "");
            break;
        case "Home/Index": ApplyClass("Home", 0, "");
            break;
        case "ManageRoles/Index": ApplyClass("Roles", 1, "Configuration");
            break;
        case "ManageUser/Index": ApplyClass("Users", 1, "Configuration");
            break;
        case "ManagePermissions/Index": ApplyClass("Permissions", 1, "Configuration");
            break;
        case "Settings/Index": ApplyClass("Settings", 1, "Configuration");
            break;
        case "ActivityLog/ActivityLogList": ApplyClass("activityLog", 1, "Configuration");
            break;
        case "ActivityLog/Index": ApplyClass("activityType", 1, "Configuration");
            break;
        case "ActivityLog/ErrorLog": ApplyClass("errorLog", 1, "Configuration");
            break;
        case "Affiliate/Index": ApplyClass("Affiliate", 0, "Configuration");
            break;
        case "Email/Index": ApplyClass("emailLog", 1, "email");
            break;
        case "Email/Templates": ApplyClass("emailTemplate", 1, "email");
            break;
    }
});

function ApplyClass(linkId, id, mainUl) {

    $("#sidebar .nav li").each(function () { $(this).removeAttr("class") });
    $("li#" + linkId).addClass("active");

    //1 means its a sub menu
    if (id == 1) {
        $("li#" + mainUl).addClass("open");
        $("li#" + linkId).addClass("active");
        $("li#" + linkId).parents("ul.submenu").show();
    }
}

function ChangeCommissionText() {

    var commissionType = $('input[name=CommissionType]:checked').val();

    if (commissionType == 1) {
        $("#lblCommission").text("Commission ($)");
    }
    else if (commissionType == 2) {
        $("#lblCommission").text("Commission (%age)");
    }
    else {
        $("#lblCommission").text("Commission ($)");
    }
}

function ConfirmSelectionBox(type) {

    var messageTitle = "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='icon-ok'></i> Selection Confirmation</h4></div>";
    $("#dialog-message").removeClass('hide').dialog({
        modal: true,
        title: messageTitle,
        buttons: [
            {
                text: "Cancel",
                "class": "btn btn-xs",
                click: function () {
                    if (type == 1) {
                        $("#TenantForm #Active").is(":checked") ? $("#TenantForm #Active").attr('checked', '') : $("#TenantForm #Active").attr('checked', 'checked');
                    }
                    else {
                        $('#TenantForm input[name=PlanType]:checked').val() == 1 ? $("#rdbPaid").attr('checked', 'checked') : $("#rdbTrial").attr('checked', '');
                        $("#trPaid").hide();
                        $(".applyClass").removeClass("required");
                    }

                    $(this).dialog("close");
                }
            },
            {
                text: "OK",
                "class": "btn btn-primary btn-xs",
                click: function () {
                    if (type == 1) {
                        $("#Active").toggleClass('checked');
                    } else {
                        $("#rdbPaid").attr('checked', this.checked ? 'checked' : '');
                        $("#rdbTrial").attr("disabled", "disabled");

                    }
                    $(this).dialog("close");
                }
            }
        ]
    }).dialog('widget').find('.ui-dialog-title').html(messageTitle);
}

function escapeHTML(text) {
    var replacements = { "<": "&lt;", ">": "&gt;", "&": "&amp;", "\"": "&quot;", "'": "&#39;", ",": "&#44", ":": "&#58;" };
    return text.replace(/[<>&"',:]/g, function (character) {
        return replacements[character];
    });
}
