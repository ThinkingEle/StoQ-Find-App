from pydantic import BaseModel
from typing import Optional, Dict, List
from datetime import datetime

# API 응답 및 요청을 위한 Pydantic 모델 (Schema)
class NodeBase(BaseModel):
    parent_id: Optional[str] = None
    type: str
    name: str
    qr_code: Optional[str] = None
    properties: Dict = {}

class NodeCreate(NodeBase):
    pass

class NodeSchema(NodeBase):
    id: str
    created_at: datetime

    class Config:
        from_attributes = True # SQLAlchemy 모델을 Pydantic으로 변환 허용