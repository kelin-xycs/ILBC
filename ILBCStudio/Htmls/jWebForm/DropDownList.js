(
    function()
    {
        $j.RegisterControlType("J:DROPDOWNLIST", DropDownList);

        function DropDownList(jelemt) {
            if (jelemt) {
                var width = jelemt.getAttribute("Width");
                var height = jelemt.getAttribute("Height");
                var selectChanged = jelemt.getAttribute("SelectChanged");
            }

            if (width)
                this.width = width;
            else
                this.width = "200px";

            if (height)
                this.height = height;
            else
                this.height = "22px";

            if (selectChanged)
                this.selectChanged = selectChanged;


            var dropDown = $j.DropDown();
            this.dropDown = dropDown;

            this.elemt = dropDown.elemt;


            var text = document.createElement("div");

            text.style.boxSizing = "border-box";
            text.style.width = this.width;
            text.style.height = this.height;
            text.style.border = "solid 1px lightblue";
            text.innerHTML = "&nbsp;";    //  Ԥ�ȸ� text div ��ֵһ���ı������� DropDownList �������ı�����һ��ʱ�����뷽ʽ����ͳһ��û���ı��� inline-block �� div ����Աߵ��ı���һ�㣬��һ��ѡ�� item �� text div ��ֵ�ı�ʱ����Աߵ��ı����룬��ʱ text div �����һ�����µ�С�ƶ�

            dropDown.Top(text);

            this.text = text;


            var list = document.createElement("div");
            list.style.boxSizing = "border-box";
            list.style.width = this.width;
            list.style.border = "solid 1px lightblue";
            list.style.backgroundColor = "white";
            list.style.cursor = "default";

            dropDown.Drop(list);

            this.list = list;


            this.selectedItem = null;

        }

        $j.DropDownList = function CreateDropDownList(id) {
            var ctrl = new DropDownList();

            if (id) {
                jwf$RegiterControl(ctrl, id);
            }

            return ctrl;
        }

        DropDownList.prototype = $j.Control();

        DropDownList.prototype.Width = function Width(width) {
            if (!width)
                return this.width;

            this.width = width;

            this.text.style.width = width;
        }

        DropDownList.prototype.Height = function Height(height) {
            if (!height)
                return this.height;

            this.height = height;

            this.text.style.height = height;
        }

        DropDownList.prototype.DataBind = function DataBind(dataSource)
        {
            var ctrl = this;
            var text = this.text;
            var list = this.list;

            for (var i = 0; i < dataSource.length; i++) {
                let item = document.createElement("div");

                item.className = "jwf_DropDownListItem";
                item.innerHTML = dataSource[i];

                item.addEventListener("mousedown",
                    function jwf$DropDownList$ItemMouseDown(e) {
                        ctrl.mousedownSelf = true;
                    }
                );

                item.addEventListener("click",
                    function jwf$DropDownList$ItemClick() {

                        text.innerHTML = item.innerHTML;
                        
                        //  ��� ��ʱ ��ʵҲ���Բ��üӣ�����ʱ�Ļ�����Ч�����һ�㣬���ӵĻ���������ʧ��̫���ˣ���Ȼ����������˶��� :)
                        window.setTimeout(
                            function jwf$DropDownList$ItemClickCloseDrop() {
                                ctrl.dropDown.Close();
                            },
                            100);

                        if (item != ctrl.selectedItem)
                        {
                            ctrl.selectedItem = item;

                            RaiseSelectChangedEvent( ctrl );
                        }
                    }
                );

                list.appendChild(item);
            }
        }

        function RaiseSelectChangedEvent( ctrl )
        {
            var handler;

            if (typeof (ctrl.selectChanged) == "string") {
                handler = window[ctrl.selectChanged];
            }
            else if (typeof (ctrl.selectChanged) == "function") {
                handler = ctrl.selectChanged;
            }

            if (handler)
                handler(ctrl);
        }

        DropDownList.prototype.SelectChanged = function SelectChanged(selectChanged) {
            if (!selectChanged)
                return this.selectChanged;

            this.selectChanged = selectChanged;
        }

        DropDownList.prototype.SelectedText = function SelectedText()
        {
            if (!this.selectedItem)
                return "";

            return this.selectedItem.innerText;
        }
    }
)();