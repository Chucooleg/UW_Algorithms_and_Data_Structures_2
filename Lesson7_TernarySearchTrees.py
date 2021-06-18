from __future__ import annotations
from typing import List, Union
from collections import deque


class TSTNode:
    def __init__(self,
                    char: str,
                    lChild: TSTNode = None,
                    midChild: TSTNode = None,
                    rChild: TSTNode = None):
        self.mCh = char
        self.isWordEnd = False
        self.mLeft = lChild
        self.mMid = midChild
        self.mRight = rChild

    def IsWordEnd(self):
        return self.isWordEnd

    def SetWordEnd(self):
        self.isWordEnd = True

class TernarySearchTree:
    def __init__(self, stringList: List[str]):
        self.mRoot = None
        for string in stringList:
            self.__AddRoot(string, 0)

    def __AddRoot(self, string: str, index: int):
        if self.mRoot is None:
            self.mRoot = TSTNode(string[index])
        if string[index] < self.mRoot.mCh:
            self.__Add(self.mRoot, 'mLeft', string, index)
        elif string[index] > self.mRoot.mCh:
            self.__Add(self.mRoot, 'mRight', string, index)
        else:
            if index < len(string) - 1:
                index += 1
                self.__Add(self.mRoot, "mMid", string, index)
            else:
                self.mRoot.SetWordEnd()
        return self.mRoot


    def __Add(self, parent: Union[TSTNode, None], attr: str, string: str, index: int):
        if getattr(parent, attr) is None:
            setattr(parent, attr, TSTNode(string[index]))  # found all equal chars, and now need to create new nodes to insert the rest of chars in str
            # Note, when we create the node above, u know that in the code below, it will
            # go into the "char equal" case, and call Add with the next char in the string.
        child_node = getattr(parent, attr)
        if string[index] < child_node.mCh:
            self.__Add(child_node, "mLeft", string, index)
        elif string[index] > child_node.mCh:
            self.__Add(child_node, "mRight", string, index)
        else:  # found the character str[index]
            if index < len(string) - 1:
                index += 1
                self.__Add(child_node, "mMid", string, index)
            else:
                child_node.SetWordEnd()
        return child_node

    def Add(self, string: str):
        self.__AddRoot(string.lower(), 0)


    def SearchRecursive(self, string: str):
        return self.__SearchRecursive(self.mRoot, string.lower(), 0)

    def __SearchRecursive(self, root: Union[TSTNode, None], string: str, index: int):
        if root is None:
            return None
        foundNode = None
        ch = string[index]
        if ch < root.mCh:
            foundNode = self.__SearchRecursive(root.mLeft, string, index)
        elif ch > root.mCh:
            foundNode = self.__SearchRecursive(root.mRight, string, index)
        else:
            if index + 1 < len(string):  # If there are more chars left to see in the string
                foundNode = self.__SearchRecursive(root.mMid, string, index+1)
            else:
                foundNode = root
        return foundNode

    def SearchIterative(self, string: str):
        string = string.lower()
        node = self.mRoot
        index = 0

        while node is not None:
            ch = string[index]

            if ch < node.mCh:
                node = node.mLeft
            elif ch > node.mCh:
                node = node.mRight
            else:
                if index + 1 < len(string):
                    index += 1
                    node = node.mMid
                else:
                    break
        return node

    def Output(self):
        self.BFSOutput()

    def BFSOutput(self):
        print()
        print("BFS output begin")

        q = deque()
        q.append(self.mRoot)
        while len(q) > 0:
            current = q.popleft()
            self.__EnqueueChildren(current, q)
            self.__OutputNode(current)
        print("BFS output end")
        print()

    def __EnqueueChildren(self, node: TSTNode, q: Queue):
        if node.mLeft is not None:
            q.append(node.mLeft)
        if node.mMid is not None:
            q.append(node.mMid)
        if node.mRight is not None:
            q.append(node.mRight)

    def __OutputNode(self, node: TSTNode):
        print(f"{node.mCh}: {node.mLeft and node.mLeft.mCh} {node.mMid and node.mMid.mCh} {node.mRight and node.mRight.mCh}")

    @staticmethod
    def DoSearches(tst: TernarySearchTree):
        ants = tst.SearchRecursive("Ants")
        antz = tst.SearchRecursive("antz")
        notFound = tst.SearchRecursive("antzzz")
        andy = tst.SearchRecursive("andy")

        ants = tst.SearchIterative("ants")
        antz = tst.SearchIterative("antz")
        notFound = tst.SearchIterative("antzzz")
        andy = tst.SearchIterative("andy")

    @staticmethod
    def DoAdds(tst: TernarySearchTree):
        tst.Output()
        tst.Add("ants")
        print("After adding ants")
        tst.Output()

        tst.Add("antz")
        print("After adding antz")
        tst.Output()

        tst.Add("anz")
        print("After adding anz")
        tst.Output()

    @staticmethod
    def Run():
        stringList = ["shells", "by", "are"]
        tst = TernarySearchTree(stringList)

        TernarySearchTree.DoAdds(tst)
        TernarySearchTree.DoSearches(tst)
