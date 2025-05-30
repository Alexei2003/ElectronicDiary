﻿using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.GroupMemberView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Lists.GroupView
{
    public class GroupViewObjectCreator : BaseViewObjectCreator<GroupResponse, GroupRequest, GroupController>
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.GroupName = _baseResponse.GroupName;
                _baseRequest.ClassRoom = _baseResponse.ClassRoom?.Id ?? -1;
            }
            else
            {
                _baseRequest.ClassRoom = _objectParentId;
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                {
                    Elem = BaseElemsCreator.CreateLabel( "Название")
                },
                _componentState == AdminPageStatic.ComponentState.Read ?
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( _baseResponse.GroupName)
                    }
                :
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor( newText =>  _baseRequest.GroupName = newText, "Английский 2",_baseResponse.GroupName )
                    }
                ]
            );

            var viewCreator = new GroupMemberViewListCreator();
            var scrollView = viewCreator.Create([], [], _baseResponse.Id, _componentState != AdminPageStatic.ComponentState.Edit, _objectParentId);
            _vStack.Add(scrollView);
        }
    }
}
