import { Component, OnInit } from '@angular/core';
import { Post } from '../../models/Post'
import { PostService } from '../../services/post.service'
import { UpdateService } from '../../services/update.service'
import { WebsocketService } from '../../services/websocket.service';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {

  posts:Post[];

  constructor(private postService:PostService, private updateService:UpdateService) { }

  ngOnInit(): void {
    //this.posts = this.postService.getPosts();
    this.postService.getPostsUrl().subscribe(posts => {
      console.log(posts);
      this.posts = posts;
    });
    this.updateService.messages.subscribe(msg => {
      console.log("Response from websocket: " + msg.Title);
      this.posts.unshift(msg);
    });
  }

  private testMessage = {
    Id: "666",
    Title: "test",
    Description: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
    ImgUrl: "https://user-images.githubusercontent.com/26636443/32541207-40f7ade8-c46f-11e7-8d74-a3435ebc3c11.png",
    Url: "https://google.com"
  }

  sendTestMsg() {
    console.log("new message from client to websocket: ", this.testMessage);
    this.updateService.messages.next(this.testMessage);
  }

}
