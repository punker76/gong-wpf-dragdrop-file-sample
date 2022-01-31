# File drag&drop

Simple File drop sample with gong-wpf-dragdrop.

```xaml
<ListBox Grid.Row="1"
         dd:DragDrop.IsDragSource="True"
         dd:DragDrop.IsDropTarget="True"
         dd:DragDrop.DragHandler="{Binding}"
         dd:DragDrop.DropHandler="{Binding}"
         dd:DragDrop.UseDefaultDragAdorner="True"
         ItemsSource="{Binding Files}">

    <ListBox.ItemTemplate>
        <DataTemplate DataType="{x:Type models:FileModel}">
            <StackPanel Orientation="Vertical" Margin="5">
                <TextBlock Text="{Binding FileName}" FontSize="18" FontWeight="Bold" />
                <TextBlock Text="{Binding File}" FontSize="12" />
            </StackPanel>
        </DataTemplate>
    </ListBox.ItemTemplate>

</ListBox>
```

## Drop files

snippet: DropFiles

## Drag files

snippet: DragFiles

## In action

![](./gong_dragdrop_files.gif)

## Credits

README generated with [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets) by @SimonCropp
