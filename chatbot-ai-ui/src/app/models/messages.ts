export class Message {
    id?: string;
    author?: Author;
    createdAt?: Date;
    content?: MessageContent;
    isCompleted: boolean = true;

    static create(id: string, author: Author, createdAt: Date, text: string, isCompleted: boolean): Message {
        return {id: id, author: author, createdAt: createdAt, content: { text: text, parts: [] }, isCompleted: isCompleted };
    }
}

export class MessageContent {
    text?: string;
    reaction?: Reaction;
    parts: number[] = [];
}

export enum Author {
    Human = 1,
    AI = 2,
}

export enum Reaction {
    ThumbsUp = 1,
    ThumbsDown
}