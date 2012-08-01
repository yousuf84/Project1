function GenerateFullScreenToggle(gridId) {
    var toolbar_button = $("<div style='float:right;margin-right:12px;cursor:pointer;'><span style='background:white;color:black'>&nbsp;View&nbsp;FullScreen&nbsp;</span>&nbsp;&nbsp;</div>");
    $(toolbar_button).appendTo("#gbox_" + gridId + " .ui-jqgrid-titlebar:first");
    $(toolbar_button).click(function () {
        $(toolbar_button).hide();
        GoFullScreen(gridId, toolbar_button);
    });
}
var jqGridColumnFilterCollection = [];
function stopPropagating(e) {
        if (!e)
            e = window.event;

        if (e.cancelBubble)
            e.cancelBubble = true;
        else
            e.stopPropagation();
}
function findFilterRules(grid, field) {
     var col =  $.grep(jqGridColumnFilterCollection, function (v, i) {
 
        return v.gridName == grid;
    })[0];
    var rules = $.grep(col.rules, function (v, i) {
        return v.field == field;
    });
    return rules;
}
function findFilterRulesByGrid(grid) {
    var col = $.grep(jqGridColumnFilterCollection, function (v, i) {

        return v.gridName == grid;
    })[0];
    var rules = $.grep(col.rules, function (v, i) {
        return true;
    });
    return rules;
}
function removeFilterRulesByName(grid, field) {
    var sets = [];
    $.each(jqGridColumnFilterCollection, function (i, v) {
        if (v.gridName == grid) {
            var new_rules = $.grep(v.rules, function (val, j) {
                return val.field != field;
            });
            v.rules = new_rules;
        }
        sets.push(v);
    });
    jqGridColumnFilterCollection = sets;
}
function setGridTemplate(name,template) {
    window.gridTemplates[name] = template;
}
function getGridTemplate(name) {
    return window.gridTemplates[name];
}
function addFilterRule(grid, field, oper, value) {
    var ind = -1;
    var filterCollection = $.grep(jqGridColumnFilterCollection, function (v, i) {
        if (v.gridName == grid) {
            ind = i;
        }
        return v.gridName == grid;
    })[0];
    if (filterCollection == null) {
        var rules = [];
        rules.push({ field: field, op: oper, data: value })
        jqGridColumnFilterCollection.push({
            gridName: grid,
            rules: rules
        });
      
    } else {
        var rules = jqGridColumnFilterCollection[ind].rules;
        rules.push({ field: field, op: oper, data: value })
        jqGridColumnFilterCollection[ind] = {
            gridName: grid,
            rules: rules
        };
    }
}
function stringFilterDropdown() {

 return   '<select class="selectopts" style="margin:5px;width:80%;"><option value="eq" selected="selected">equal</option><option value="ne">not equal</option><option value="bw">begins with</option><option value="bn">does not begin with</option><option value="ew">ends with</option><option value="en">does not end with</option><option value="cn">contains</option><option value="nc">does not contain</option><option value="nu">is null</option><option value="nn">is not null</option><option value="in">is in</option><option value="ni">is not in</option></select>';
}
function numberFilterDropdown() {
    return '<select class="selectopts" style="margin:5px;width:80%;"><option value="eq">equal</option><option value="ne">not equal</option><option value="lt">less</option><option value="le">less or equal</option><option value="gt">greater</option><option value="ge">greater or equal</option><option value="nu">is null</option><option value="nn">is not null</option><option value="in">is in</option><option value="ni">is not in</option></select>';
}

function jqFilterClick(elem,template) {
    var col = $(elem).closest("[id^=jqgh_]");
    $(".filter-popup").remove();
    var grid = $(col).attr("id").split('_')[1];
    var name = $(col).attr("id").split('_')[2];
    var dropText = "";

    var type = $("#" + grid).jqGrid('getColProp', name).sorttype;

    if (type == "float" || type == "date") {
        dropText = numberFilterDropdown();
    } else {
    dropText = stringFilterDropdown();
    }
var filter_pop = $("<div class='filter-popup Silver-Gradient-Filter' data-gridIDName='" + grid + "' data-gridFieldName='" + name + "'><p class='jqfilter-title'>Filter for " + name + " :" + 
  "<div style='padding:5px;border-top:1px solid #F2F2F2'><div style='margin-top:5px;'><i>filter 1:</i><br />oper:" + dropText + "</div>" +
  "<input type='text' size='40' /><div style='height:2px;width:98%;background:#C4CBCF;margin:6px 0 4px 0'></div>" +
  "<div><i>filter 2:</i><br/>oper:" + dropText + "</div><input type='text' size='40' />" +
  "<table style='width:100%;' cellspacing='6'><tr style='width:100%;'><td>" +
   "<button class='jqfilter-button-clear' style='width:100%;height:60px;line-height:60px' onclick='filterDialog_clear(this)'>Clear</button>" + 
  "</td><td>"+  
  "<button class='jqfilter-button' style='width:100%;height:60px;line-height:60px' onclick='filterDialog_trigger(this)'>Apply Filter</button>" +
  "</td></tr></table></div>")

    $("body").append(filter_pop);
    setTimeout(function () {
        $(filter_pop).position({
            my: "left top",
            at: "left bottom",
            of: col,
            offset: "-3 4"

        });
        var postData = $("#" + grid).jqGrid("getGridParam", "postData");
        var firstDrp = $(filter_pop).find("select:first");
        var sndDrp = $(filter_pop).find("select:last");
        var txt1 = $(filter_pop).find("input[type=text]:first");
        var txt2 = $(filter_pop).find("input[type=text]:last");
        var rules = findFilterRules(grid, name);
        $(firstDrp).find("option[value='" + rules[0].op + "']").attr("selected", "selected");
        $(sndDrp).find("option[value='" + rules[1].op + "']").attr("selected", "selected");
        $(txt1).val(rules[0].data);
        $(txt2).val(rules[1].data);


    }, 100);
}
function filterDialog_clear(btn) {
    var p = $(btn).closest(".filter-popup");
    $(p).find("select option").removeAttr("selected");
    $(p).find("input[type=text]").val("");
}
function filterDialog_trigger(btn) {
    var pop = $(btn).closest(".filter-popup");
    var drp1 = $(pop).find("select:first");
    var drp2 = $(pop).find("select:last");
    var txt1 = $(pop).find("input:first");
    var txt2 = $(pop).find("input[type=text]:last");
    var grid = $(pop).attr("data-gridIDName");
    var field = $(pop).attr("data-gridFieldName");
    removeFilterRulesByName(grid, field);
    if ($(txt1).val() != null) {
        addFilterRule(grid, field, $(drp1).val(), $(txt1).val());
    }
    if ($(txt2).val() != null) {
        addFilterRule(grid, field, $(drp2).val(), $(txt2).val());
    } var searchData = {};
    searchData.filters = {};
    searchData.filters.rules = [];
    searchData.filters.rules = findFilterRulesByGrid(grid);
    searchData.filters.groupOp = "AND";
    searchData.searchField = "";
    searchData.searchOper = "";
    searchData.searchString = "";
    searchData.filters.rules = $.grep(searchData.filters.rules, function (v, i) {
        return v.data != null && v.data != "";
    });
    searchData.filters = xmlJsonClass.toJson(searchData.filters).replace(/undefined/g, '');
    $("#" + grid).jqGrid('setGridParam', { postData: searchData, search: true })
    $("#" + grid).trigger("reloadGrid");
    setTimeout(function () {
        $(pop).remove();
    }, 80);
}
function GoFullScreen(gridId, toolbar_button) {
    var width = $(window).width();
    var height = $(window).height();
    var originalWidth = $("#gbox_" + gridId).width();
    var originalHeight = $("#gbox_" + gridId).height();
    var interval = setInterval(function () {
        var current_width = $("#gbox_" + gridId).width();
        var current_height = $("#gbox_" + gridId).height();
        $("#" + gridId).jqGrid('setGridWidth', (current_width), true);
        $("#" + gridId).jqGrid('setGridHeight', (current_height) + "px", true);
    }, 10);
    $("#gbox_" + gridId).animate({ width: width - 63, height: height * 0.80 }, 300, "swing", function () {
        interval = null;
    });
    $("#gbox_" + gridId).wrap("<div id='s-wrapper" + gridId + "' class='full-screen'></div>");
    $("#s-wrapper" + gridId).attr("style", " position:relative; z-index:10000;border:1px solid black -webkit-border-radius: 10px;-moz-border-radius: 10px;border-radius: 10px;   background-size:100%;height: 92%;margin: 0;background-repeat: no-repeat;background: #fcfcfc; /* Old browsers */background: -moz-linear-gradient(top, #fcfcfc 0%, #fbfbfb 29%, #ffffff 100%); /* FF3.6+ */background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#fcfcfc), color-stop(29%,#fbfbfb), color-stop(100%,#ffffff)); /* Chrome,Safari4+ */background: -webkit-linear-gradient(top, #fcfcfc 0%,#fbfbfb 29%,#ffffff 100%); /* Chrome10+,Safari5.1+ */background: -o-linear-gradient(top, #fcfcfc 0%,#fbfbfb 29%,#ffffff 100%); /* Opera11.10+ */background: -ms-linear-gradient(top, #fcfcfc 0%,#fbfbfb 29%,#ffffff 100%); /* IE10+ */filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fcfcfc', endColorstr='#ffffff',GradientType=0 ); /* IE6-9 */background: linear-gradient(top, #fcfcfc 0%,#fbfbfb 29%,#ffffff 100%); /* W3C */   padding:24px;padding-top:36px");
    $("#s-wrapper" + +gridId + " fieldset").hide();
    var text = $("#gbox_" + gridId).find(".ui-jqgrid-title:first").text();
    $("#gbox_" + gridId).find(".ui-jqgrid-titlebar:first").hide();
    $("#s-wrapper" + gridId).append('<div style="display:none;cursor:pointer;position:absolute;right:32px;top:8px;font-size:14px;font-weight:bold;color:#C90606;" class="exit-full' + gridId + '">X Close</div>');
    $("#s-wrapper" + gridId).append('<div style="display:none;cursor:pointer;position:absolute;left:46px;top:4px;font-weight:bold;font-size:22px;" class="popup-title' + gridId + '">' + text + '</div>');
    $("#s-wrapper" + gridId).css({ position: 'fixed', top: '6px', left: '6px' });
    $(".popup-title" + gridId).show();
    $(".exit-full" + gridId).show();

    var handler = $(window).bind('resize.GRID', function () {
        var handler_width = $(window).width();
        var handler_height = $(window).height();
        $("#" + gridId).jqGrid('setGridWidth', handler_width - 66, true);
        $("#" + gridId).jqGrid('setGridHeight', handler_height * 0.80 + "px", true);
    });
    $(".exit-full" + gridId).click(function () {
        LeaveFullScreen(gridId, originalWidth, originalHeight, toolbar_button, handler);
    });
}

$(document).ready(function () {
    $(".filterIcon").live("click", function () { alert("test..."); });
    $(".view-fullscreen").click(function () {
        $(this).hide();
        GoFullScreen($(this).attr("data-gridId"), this);
    });

    setInterval(function () {
        $(".ui-tabs").find("[id^=gbox_]").each(function () {
            var gridName = $(this).attr("id").split("_")[1];
            $("#" + gridName + "_t").jqGrid("setGridWidth", "100px");
        });
    }, 100);
});

function LeaveFullScreen(gridId, w, h, toolbar_button, handler) {
    $("#s-wrapper" + gridId + " fieldset").show();
    $("#gbox_" + gridId).find(".ui-jqgrid-titlebar:first").show();
    $(".exit-full" + gridId).remove();
    $(".popup-title" + gridId).remove();
    $("#gbox_" + gridId).unwrap();
    setTimeout(function () {
        $("#" + gridId).jqGrid('setGridWidth', w, true);
        $("#" + gridId).jqGrid('setGridHeight', h, true);
    }, 25);
    $(toolbar_button).show();
    $(window).unbind('resize.GRID');
}
var unique_count = 0;
function MoveColumnToCaptureBox(field, GridName) {
    var captured_item_template = $(".capture-box").find(".captured-item-template").html();
    captured_item_template = $("<div class='captured-item' data-captureName='" + field + "' data-gridCaptureName='" + GridName + "'>" + captured_item_template.replace('${column}', field) + "</div>");
    $(captured_item_template).insertAfter($(".capture-box").find(".capture-box-tag"));
    jQuery("#" + GridName).jqGrid('hideCol', field).trigger("reloadGrid");
}

jQuery.extend(jQuery.expr[':'], {
    NotGrouped: function (el) {
        if (inGroupedQuery) {
            return true;
        } else {
            return $(el).parents("[data-groupQuery]").size() == 0;
        }
    }
});
var inGroupedQuery = false;

var jqGMVC = {
    getTopLevelGroupTrees: function (grid) {

        return $.grep($("[data-groupQuery='" + grid + "']"), function (v, i) { return $(v).parents("[data-groupQuery]").size() == 0; });
    },
    buildGroupTreeQuery: function (grid, fields) {
        var tree = [];
        var groupTrees = this.getTopLevelGroupTrees(grid);
        var that = this;
        $.each(groupTrees, function (i, v) {
            var branch = that.buildGroupBranch(grid, fields, v).filters;
            tree.push(branch);
        });
        return tree;
    },
    buildGroupBranch: function (grid, fields, container) {
        var clone = $(container).clone();
        $(clone).find("[data-groupQuery]").remove();
        var that = this;
        var branch = buildSearchData(null, grid, fields, -1, false, clone);
        branch.filters.groups = [];
        $(container).find("[data-groupQuery='" + grid + "']").each(function (i, v) {
            var sub_branch = that.buildGroupBranch(grid, fields, v).filters;
            if (sub_branch.rules.length > 0) {
                branch.filters.groups.push(sub_branch);
            }
        });
        return branch;

    }
};

function buildSearchData(url, gridName, fields, count, toolbarSearch, groupQuery) {

    if (groupQuery != null) {
        inGroupedQuery = true;
    } else {
    inGroupedQuery = false;
    }
    var nm_mask = jQuery("#item_nm").val();
    var cd_mask = jQuery("#search_cd").val();
    var constructQuery = "";
    var searchForm = document;
    if (groupQuery == null) {
        if ($("[data-gridForm='" + gridName.replace('-', ' ') + "']").size() > 0) {
            searchForm = $("[data-gridForm='" + gridName.replace('-', ' ') + "']");
        }
        if (toolbarSearch) {
            searchForm = $("#gbox_" + gridName.replace(' ', '-'));
        }
    } else {
    searchForm = $(groupQuery);
    }
     var searchData = {};
    searchData.filters = {};
    searchData.filters.rules = [];
    searchData.filters.groupOp = "AND";
    if (groupQuery != null) {
      searchData.filters.groupOp = $(groupQuery).attr("data-gridBoolean");
    }
    searchData.searchField = "";
    searchData.searchOper = "";
    searchData.searchString = "";
    var $grid = $("#" + gridName.replace(' ', '-'));

    $.each(new String(fields).split(','), function (i, v) {
        var values = [];
        $("#" + v + "_search", searchForm).not("[type=checkbox]").filter(":NotGrouped").each(function () {
            if ($(this).val() != null && $(this).val() != "") {
                values.push($(this).val());
            }
        });
        if (values.length == 0) {
            $("[name=" + v + "_search]", searchForm).not("[type=checkbox]").filter(":NotGrouped").each(function () {
                if ($(this).val() != null && $(this).val() != "") {
                    values.push($(this).val());
                }
            });
        }
        if (values.length == 0) {

            $("#gs_" + v + "", searchForm).filter(":NotGrouped").each(function () {
                if ($(this).val() != null && $(this).val() != "") {
                    values.push($(this).val());
                }
            });

        }

        if (values.length == 0) {

            $("#" + v + "_search,[name='" + v + "_search']:checked", searchForm).filter(":NotGrouped").each(function () {

                if ($(this).attr("data-value") != null) {
                    values.push($(this).attr("data-value"));
                } else {
                    values.push($(this).is(":checked") ? "on" : "off");
                }
            });
        }


        $.each(values, function (i, val) {
            searchData.filters.rules.push({
                field: v,
                op: "cn",
                downPath: false,
                data: val
            });
        });
    });
    $('[name^="To$"]', searchForm).filter(":NotGrouped").each(function () {
        if ($(this).val().length > 5) {
            searchData.filters.rules.push({
                field: $(this).attr("name").split("$")[1].replace('_search', ''),
                op: "le",
                downPath: false,
                data: $(this).val()
            });
        }
    });
    $('[name^="From$"]', searchForm).filter(":NotGrouped").each(function () {
        if ($(this).val().length > 5) {
            searchData.filters.rules.push({
                field: $(this).attr("name").split("$")[1].replace('_search', ''),
                op: "ge",
                downPath: false,
                data: $(this).val()
            });
        }
    }); $('[name^="ToNumber$"]', searchForm).filter(":NotGrouped").each(function () {
        if ($(this).val().length >= 1) {
            searchData.filters.rules.push({
                field: $(this).attr("name").split("$")[1].replace('_search', ''),
                op: "le",
                downPath: false,
                data: $(this).val()
            });
        }
    });
    $('[name^="FromNumber$"]', searchForm).filter(":NotGrouped").each(function () {
        if ($(this).val().length >= 1) {
            searchData.filters.rules.push({
                field: $(this).attr("name").split("$")[1].replace('_search', ''),
                op: "ge",
                downPath: false,
                data: $(this).val()
            });
        }
    });
    $('[name^="Time_From$"]', searchForm).filter(":NotGrouped").each(function () {
        searchData.filters.rules.push({
            field: $(this).attr("name").split("$")[1],
            op: "ge",
            downPath: false,
            timeRange: true,
            data: $(this).val()
        });
    });
    $('[name^="Time_To$"]', searchForm).each(function () {
        searchData.filters.rules.push({
            field: $(this).attr("name").split("$")[1],
            op: "le",
            downPath: false,
            timeRange: true,
            data: $(this).val()
        });
    });
    $('[name^="Custom$"]', searchForm).filter(":NotGrouped").each(function (i, v) {
        if ($(this).attr("data-relation") != null) {
            var value = $(this).val();
            constructQuery += "&" + $(this).attr("name") + "=" + value;
        } else {
            var value = $(this).val();
            if ($(this).is("[type=checkbox]")) {
                value == $(this).is(":checked") ? "true" : "false";
            }
            searchData.filters.rules.push({
                field: $(this).attr("name").split("$")[1],
                op: "custom",
                downPath: false,
                data: value,
                custom:true
            });
        }
    });
    $("[type=checkbox][name$='Null']", searchForm).filter(":NotGrouped").each(function () {
        var value = $(this).is(":checked");
        if (value) { value = "on"; } else { value = "off"; }
        searchData.filters.rules.push({
            field: $(this).attr("name"),
            op: "=",
            downPath: false,
            data: value
        });
    });
    $('[name^="search$"]', searchForm).filter(":NotGrouped").each(function (i, v) {
        var value = $(this).val();
        var split = $(this).attr("name").split("$");
        searchData.filters.rules.push({
            field: split[1],
            op: "cn",
            downPath: true,
            data: value
        });
    });
    searchData.filters.rules = $.grep(searchData.filters.rules, function (v, i) {
        return v.data != null && v.data != "";
    });
  
    if (groupQuery == null) {
        searchData.filters.groups = jqGMVC.buildGroupTreeQuery(gridName, fields);


    }
    if (groupQuery == null) {
        searchData.filters = xmlJsonClass.toJson(searchData.filters).replace(/undefined/g, '');
        return searchData;
    } else {
    return searchData;
    }
}
function JQGrid_GoToPage(grid, page) {
    $(grid).trigger("reloadGrid", [{ page: page}]);
}
function JQGrid_FindPage(grid) {

    return $(grid).jqGrid("getGridParam","page");
}
function gridReload(url, gridName, fields, count) {
    var searchData = buildSearchData(url, gridName, fields, count);
    var $grid = $("#" + gridName.replace(' ', '-'));
    if ($grid.jqGrid('getGridParam', 'loadonce') == true) {
        $grid.jqGrid('setGridParam', { postData: searchData, search: true })
        $($grid).trigger("reloadGrid", [{ page: 1}]);
    } else {
        $grid.jqGrid('setGridParam', { postData: searchData, search: true, url: url + "?isSearch=true&columnOrder=" + fields + "&rows=" + count });
        $($grid).trigger("reloadGrid", [{ page: 1}]);
    }
}
function SyncCustomGridPager(grid, container) {

    var records = $("#" + grid).jqGrid("getGridParam", "records");
    var rowNum = $("#" + grid).jqGrid("getGridParam", "rowNum");
    $(".grid-current-page",container).text(JQGrid_FindPage("#" + grid));
    $(".grid-record-count",container).text(records);
    $(".grid-page-count",container).text(Math.ceil(records / rowNum));
    $(".grid-record-range-bottom",container).text((JQGrid_FindPage("#" + grid) - 1) * rowNum);
    $(".grid-record-range-top", container).text(((JQGrid_FindPage("#" + grid) - 1) * rowNum) + $("#" + grid).jqGrid("getGridParam", "reccount"));
    $(".next-page",container).unbind("click.next").bind("click.next", function () {
        var current_page = JQGrid_FindPage("#" + grid);
        JQGrid_GoToPage("#" + grid, current_page + 1);
    });
    $(".prev-page",container).unbind("click.prev").bind("click.prev", function () {
        var current_page = JQGrid_FindPage("#" + grid);
        JQGrid_GoToPage("#" + grid, current_page - 1);
    });
    $(".last-page", container).unbind("click.last").bind("click.last", function () {
        JQGrid_GoToPage("#" + grid, Math.ceil(records / rowNum));
    });
    $(".first-page", container).unbind("click.first").bind("click.first", function () {
        JQGrid_GoToPage("#" + grid, 1);
    });
    var template = $(".page-icon-template", container);
    var iconHtml = template.html();
    template.parent().find(".page-icon").remove();
    var iconCount = template.attr("data-icon-count");
    var cPage = JQGrid_FindPage("#" + grid);
    var ciel = cPage + Math.ceil(iconCount / 2);
    if (cPage - Math.floor(iconCount / 2) > 0) {
        var seed = JQGrid_FindPage("#" + grid) - Math.floor(iconCount / 2);
    } else {
    var seed = 1;
        ciel = seed +  parseInt(iconCount);
    }
    if (ciel > Math.ceil(records / rowNum)) {
        ciel = Math.ceil(records / rowNum) + 1;
    }


    for (var i = seed; i < ciel; i++) {

    var newHtml = $(iconHtml.replace("${icon-text}", i))
    $(newHtml).insertBefore(template);
    $(newHtml).addClass("page-icon");
    $(newHtml).attr("page-icon-number", i);
    $(newHtml).attr("page-icon-grid", grid);
    if (i == cPage) {
        $(newHtml).addClass("jqGrid-currentPage");
    } else {
    var new_index = i.toString()
    }

}

}
var captured_column_drag = null;
var column_captured_in_bar = null;
var captured_column_grid = null;
var captured_item_drag = null;
var preventFilterCapture = false;
$(document).ready(function () {


    $(".page-icon-template").hide();
    $(".page-icon").live("click", function () {
        var page = $(this).attr("page-icon-number");
        JQGrid_GoToPage("#" + $(this).attr("page-icon-grid"), page);
    });
    $(".filter-popup").live('click', function (e) {
        preventFilterCapture = true;
        setTimeout(function () {
            preventFilterCapture = false;
        }, 20);
        e.stopPropagation();
    });
    $("body").live('click', function () {
        if (!preventFilterCapture) {
            $(".filter-popup").remove();
        } else { preventFilterCapture = false; }
    });


    $(".captured-item-template").hide();
    $(".ui-th-column").live("mousedown", function () {
        $(".capture-box").addClass("column-dragged");
        if (captured_column_drag == null) {
            captured_column_drag = $(this);
            captured_column_grid = $(this).closest(".ui-jqgrid-view").attr("id").split("gview_")[1];
        }
    });
    $(document).mouseup(function () {
        captured_column_drag = null;
        $(".capture-box").removeClass("capture-box-hover");
        $(".column-dragged").removeClass("column-dragged");
    });

    $(".capture-box").live("mouseenter", function () {

        if (captured_column_drag != null) {
            column_captured_in_bar = $(captured_column_drag).attr("id").split('_')[1];
            $(this).addClass("capture-box-hover");
        }
    });
    $(".capture-box").live("mouseleave", function () {

        if (captured_column_drag != null) {
            $(this).removeClass("capture-box-hover");
            column_captured_in_bar = null;
        }

    });
    $(".capture-box").live("mousemove", function () {
        if ($(this).find(".captured-item").size() == 0) {
            $(".capture-box-tag").show();
        }
    });
    $(".capture-box").live("mouseup", function () {
        if (column_captured_in_bar != null) {
            var that = this;

            $("#" + captured_column_grid).children().sortable('cancel');
            setTimeout(function () {
                MoveColumnToCaptureBox(column_captured_in_bar, captured_column_grid);
                captured_column_drag = null;
                captured_column_grid = null;
                column_captured_in_bar = null;

            }, 100);

        }
    });

    $(".captured-item").live('click', function () {
        var gridName = $(this).attr("data-gridCaptureName");
        var column = $(this).attr("data-captureName");
        jQuery("#" + gridName).jqGrid('showCol', column).trigger("reloadGrid");
        $(this).remove();
    });


    /*
    $(".captured-item").live('mouseenter', function () {
            
    var gridName = $(this).attr("data-gridCaptureName");
    var column = $(this).attr("data-captureName");
    jQuery("#" + gridName).jqGrid('showCol', column).trigger("reloadGrid");       
    });
    $(".captured-item").live('mouseleave',function(){
    var gridName = $(this).attr("data-gridCaptureName");
    var column = $(this).attr("data-captureName");
    jQuery("#" + gridName).jqGrid('hideCol', column).trigger("reloadGrid");
    });
    */
    setInterval(function () {
        $("[data-jqcommand]").unbind("click.jqGrid");
        $("[data-jqcommand]").bind('click.jqGrid', function () {
            var cell = $(this).closest("[role=row]");
            var gridName = $(this).closest("[id^=gbox_]").attr("id").split("_")[1];
            var grid = $("#" + gridName);
            var rowid = $(cell).attr("id");
            var command = $(this).attr("data-jqcommand");
            if (command == "Delete") {
                if ($(this).attr("data-jqCommandText") == "" || confirm($(this).attr("data-jqCommandText"))) {

                    $.ajax({
                        url: location.href,
                        data: { id: rowid, oper: 'delete' },
                        type: "POST",
                        success: function (response) {
                            $(grid).jqGrid('delRowData', rowid);
                        }
                    });

                }
            }
            else if (command == "Edit") {
                jQuery(grid).jqGrid('editRow', rowid, true);
                var props = $(grid).jqGrid('getColProp', 'PostDate');
                var type = props["sorttype"];
                var columns = [];
                $.each($(grid).jqGrid('getRowData'), function (i, v) {
                    for (var p in v) {

                        if ($(grid).jqGrid('getColProp', p).sorttype == "date") {
                            var name = rowid + "_" + p;
                            setTimeout(function () {
                                $("#" + name).datepicker();
                            }, 100);
                        }
                    }
                });
                $(this).hide(); $(cell).find("[data-jqcommand=Delete]").hide();
                $(cell).find("[data-jqcommand=SaveChanges],[data-jqcommand=Cancel]").show().css("display", "inline-block");
            } else if (command == "Cancel") {
                jQuery(grid).jqGrid('restoreRow', rowid, true);
                $(cell).find("[data-jqcommand=Edit]").show().css("display", "inline-block");
                $(cell).find("[data-jqcommand=SaveChanges],[data-jqcommand=Cancel]").hide();
                $("[data-jqcommand=Delete]").show();
            } else if (command == "SaveChanges") {
                jQuery(grid).jqGrid('saveRow', rowid, true);
                $(cell).find("[data-jqcommand=Edit]").show().css("display", "inline-block");
                $(cell).find("[data-jqcommand=SaveChanges],[data-jqcommand=Cancel]").hide();
                $(cell).find("[data-jqcommand=Delete]").show();
            }
        });
    }, 500);



});
function injectContentIntoRow(gridId, html, index) {
    html = new String(html).replace(/\*/g, "\"");
    $("#" + gridId).find("tr[role=row]").not("tr[role=row]:first").each(function (i, v) {
        $(this).find(":nth-child(" + index + ")").html(html);
    });
}



