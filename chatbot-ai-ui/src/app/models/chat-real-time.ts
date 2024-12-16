export const RECEIVED_MESSAGE = 'received';
export const APPEND_TEXT_MESSAGE = 'appendText';
export const DONE_MESSAGE = 'done';

export class SendMessage {
    text: string = '';
}

export class ReceivedMessage {
    messageId?: string;
    createdAt?: Date;
    responseId?: string;
    responseCreatedAt?: Date;
}

export class StartMessage {
    messageId?: string
    responseId?: string
}

export class AppendTextMessage {
    partId?: number;
    messageId?: string;
    text?: string;
}

export class CompletedMessage {
    messageId?: string;
}