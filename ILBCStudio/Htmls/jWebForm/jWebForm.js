(
    function ()
    {
        document.documentElement.setAttribute("ondragstart", "return false;");

        window.addEventListener("load", window_load);
        //window.addEventListener("mousedown", window_mousedown);

        /* �ⲿ���� Ϊ�˽�� window mousedown �¼� �� iframe �� �������� �� ����
           iframe ��һ�� ������ window �� ��� iframe ����ֻ�ᴥ�� iframe �����Լ��� mousedown �¼�
           �����͵��� ������ �� DropDown DropDownList DropMenu �� �ؼ� �������� window mousedown �¼� �ر� ������ �����˵�
           ʹ�÷��� �ǣ�
        
            1 jWebForm �ؼ� ʹ�� jwf$AddEventHandler_To_Frames_Window_MouseDown(handler) ���� �� ��� window mousedown �¼� 
              ���� window.addEventListener("mousedown", function xxx());
        
            2 ������Ա Ӧ���� �� $j.Frame_Window_MouseDown ��ӵ� iframe �� window mousedown �¼������£�
        
              var ifr = document.getElementById("ifr");
              ifr.contentWindow.addEventListener("mousedown", $j.Frame_Window_MouseDown);
        
            ��Ȼ���� 2 �� ���Ǳ���ģ�������� �� 2 ������ô��jWebForm �Ͳ�֪�� ��� iframe ���¼�������
            ���µ�Ч������ ���� ��� iframe ʱ ������ ��� DropDown DropDownList DropMenu �� ������ �����˵� ����ر�
            ��Ȼ�ⲻһ�������⣬��ʱ��������Ч��Ҳ�ǿ��Խ��ܵģ�������ʱ��Ҫ�ľ�������Ч���� ^^
        
           �ƶ���֮�����һ��ҳ���а������ iframe�� iframe Ҳ��Ƕ�ף���ôҲ����������������
           ���ǰ� ������ �� iframe ��������һ�� frame��
           ����һ�� ҳ�� ���� n �� frame��
           ������Ա �� A frame ��ʹ���� jWebForm �ؼ���
           �����ѡ�� A frame �� $j.Frame_Window_MouseDown ��ӵ� ����������� frame �� window mousedown �¼���
           �� B frame �� C frame ���� �� ʹ���� jWebForm �ؼ���Ȼ���������ơ�
           ����ͬʱ�ڶ�� frame ��ͬʱʹ�� jWebForm �ؼ���
        */
        

        //function AddEventHandler_To_Frames_Window_MouseDown(handler) {
        //    window_mousedown$handlers[window_mousedown$handlers.length] = handler;
        //}

        
        /*  *****************************************  */

        var controls = new Object();

        function $j(id) {
            return controls[id];
        }

        function window_load() {
            InitControls();

            if ($j.Page_Load) {
                $j.Page_Load();
            }
        }

        function InitControls() {
            var jelemts = [];

            GetJwfElements(jelemts, document.body);

            for (var i = 0; i < jelemts.length; i++) {
                var jelemt = jelemts[i];

                var ctrl = GetControl(jelemt);

                var id = jelemt.getAttribute("id");

                if (id) {
                    RegisterControl(ctrl, id);
                }

                jelemt.parentNode.replaceChild(ctrl.elemt, jelemt);

            }
        }

        // ��Ϊ document  document.documentElement  document.body �� getElementsByTagNameNS() �����������ã�
        // ���� �ռ��ϣ�����ֻ�ܵݹ���������� jWebForm��j:�� Ԫ�ء�
        function GetJwfElements(elemts, elemt) {
            var s = elemt.nodeName.substring(0, 2);
            if (elemt.nodeName.substring(0, 2) == "J:") {
                elemts[elemts.length] = elemt;
            }

            if (elemt.childNodes.length == 0)
                return;

            for (var i = 0; i < elemt.childNodes.length; i++) {
                var childNode = elemt.childNodes[i];

                GetJwfElements(elemts, childNode);
            }
        }

        function RegisterControl(ctrl, id) {
            ctrl.id = id;
            ctrl.elemt.id = id;

            controls[id] = ctrl;
        }

        var controlTypes = new Object();

        function RegisterControlType(nodeName, type)
        {
            controlTypes[nodeName] = type;
        }

        
        function GetControl(jelemt) {
            var ctor = controlTypes[jelemt.nodeName];

            if (!ctor)
                throw "δע��� nodeName ��\"" + jelemt.nodeName + "\"����ʹ�� RegisterControlType(nodeName, type) ע�� ��";

            return new ctor(jelemt);

            //if (jelemt.nodeName == "J:DROPDOWNLIST")
            //{
            //    return new jwf$DropDownList(jelemt);
            //}
            //else if (jelemt.nodeName == "J:PICTUREBOX")
            //{
            //    return new jwf$PictureBox(jelemt);
            //}
            //else if (jelemt.nodeName == "J:BUTTON")
            //{
            //    return new jwf$Button(jelemt);
            //}
            //else if (jelemt.nodeName == "J:DROPMENU")
            //{
            //    return new jwf$DropMenu(jelemt);
            //}

            //throw "��Ч�� nodeName ��\"" + jelemt.nodeName + "\" ��";
        }

        function Control() {

        }

        Control.prototype.Element = function Element() {
            return this.elemt;
        }

        
            
        //$j.Frame_Window_MouseDown = window_mousedown;
        $j.RegisterControlType = RegisterControlType;
        $j.Control = function CreateControl()
        {
            return new Control();
        }

        $j.getElementById = function getElementById(containerElement, id)
        {
            
            var child = containerElement.firstElementChild;

            if (!child)
                return null;

            while (true)
            {
                if (child.id == id)
                    return child;

                

                var temp = getElementById(child, id);

                if (temp)
                    return temp;
                
                if (!child.nextElementSibling)
                    break;

                child = child.nextElementSibling;
            }

            return null;
        }

        //var window_mousedown$handlers = [];

        //  Ϊ��ʵ�� �� frame �� ȫ���¼�������ʹ�� $j.addEventListener(type, listener) ����
        //  Ϊ���� ȫ���¼� ��Ч����Ҫ���� $j.RegisterFrame(win) ������ frame window ע�ᵽ $j
        //  ������ jWebForm.js ��ҳ����Զ����� $j.RegisterFrame(win) ����ǰ window ע�ᵽ $j
        //  δ���� jWebForm.js �� frame ҳ����Ҫ�ֶ����� $j.RegisterFrame(win) �� frame window ע�ᵽ $j
        //  $j.RegisterFrame(win) �����������κ� frame �е��ã��÷������ҵ� ���㴰�ڣ�top window������ ע����Ϣ ������ top window
        $j.addEventListener = function addEventListener(type, listener) {

            var handlers = top.jwf$frameEvents[type];

            if (!handlers)
                throw "��δ֧�� \"" + type + "\" �¼� ��";

            handlers.Add(listener);
        }
            

        $j.removeEventListener = function removeEventListener(type, listener)
        {
            var handlers = top.jwf$frameEvents[type];

            handlers.MoveToStart();

            while (handlers.MoveNext())
            {
                var node = handlers.Current();
                var h = node.value;

                if (h == listener)
                {
                    handlers.Remove(node);
                    break;
                }
            }
        }
        //function window_mousedown() {
        //    for (var i = 0; i < window_mousedown$handlers.length; i++) {
        //        var handler = window_mousedown$handlers[i];

        //        handler();
        //    }
        //}

        function RaiseEvent(handlers, e)
        {
            handlers.MoveToStart();

            while (handlers.MoveNext())
            {
                var handler = handlers.Current().value;

                handler(e);
            }
        }

        function GetTopWindow(win) {
            if (win.parent == win)
                return win;

            return GetTopWindow(win.parent);
        }


        function RegisterFrame(win)
        {
            win.addEventListener("mousedown", frame_mousedown);
            win.addEventListener("mousemove", frame_mousemove);
            win.addEventListener("mouseup", frame_mouseup);
        }

        function frame_mousedown(e)
        {
            var handlers = top.jwf$frameEvents["mousedown"];

            RaiseEvent(handlers, e);
        }

        function frame_mousemove(e) {
            var handlers = top.jwf$frameEvents["mousemove"];

            RaiseEvent(handlers, e);
        }

        function frame_mouseup(e) {
            var handlers = top.jwf$frameEvents["mouseup"];

            RaiseEvent(handlers, e);
        }
        //function AddEventHandler_To_Frames_Window_MouseDown(handler) {
        //    window_mousedown$handlers[window_mousedown$handlers.length] = handler;
        //}

        $j.RegisterFrame = RegisterFrame;

        window.$j = $j;


        var top = GetTopWindow(window);

        if (window == top) {
            window.jwf$frameEvents = new Object();
            window.jwf$frameEvents["mousedown"] = new LinkedList();
            window.jwf$frameEvents["mousemove"] = new LinkedList();
            window.jwf$frameEvents["mouseup"] = new LinkedList();
        }

        RegisterFrame(window);


        function LinkedList() {

            this.first = null;
            this.last = null;
            this.current = null;
        }

        function LinkedListNode(value) {
            this.before = null;
            this.next = null;

            this.value = value;
        }

        LinkedList.prototype.Add = function Add(value) {
            var node = new LinkedListNode(value);

            if (this.last) {
                this.last.next = node;
                node.before = this.last;
                this.last = node;

                return;
            }

            this.first = node;
            this.last = node;
        }

        LinkedList.prototype.Remove = function Remove(node) {

            if (node == this.first && node == this.last) {
                this.first = null;
                this.last = null;


            }

            else if (node == this.first) {

                this.first = node.next;


            }

            else if (node == this.last) {

                this.last = node.before;


            }
            else {
                node.before.next = node.next;
                node.next.before = node.before;
            }

            if (this.current == node)
                this.current = node.before;
        }

        LinkedList.prototype.MoveToStart = function MoveToStart() {
            this.current = null;
        }

        LinkedList.prototype.MoveNext = function MoveNext() {
            if (this.current == null) {
                if (this.first == null)
                    return false;

                this.current = this.first;
                return true;
            }

            if (this.current.next != null) {
                this.current = this.current.next;
                return true;
            }

            return false;
        }

        LinkedList.prototype.Current = function Current() {
            return this.current;
        }
    }
)();




