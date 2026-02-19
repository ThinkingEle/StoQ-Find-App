from fastapi import FastAPI, HTTPException, Depends
from sqlalchemy.orm import Session
from typing import List, Optional
import database as db_model

app = FastAPI()

# DB 세션 의존성 주입
def get_db():
    db = db_model.SessionLocal()
    try:
        yield db
    finally:
        db.close()

@app.post("/nodes", response_model=db_model.NodeSchema)
def create_node(node: db_model.NodeCreate, db: Session = Depends(get_db)):
    db_node = db_model.Node(**node.model_dump())
    db.add(db_node)
    db.commit()
    db.refresh(db_node)
    return db_node

# 1. 노드 목록 조회 (계층 구조 지원)
@app.get("/nodes", response_model=List[db_model.NodeSchema]) # NodeSchema는 pydantic 모델로 가정
def read_nodes(parent_id: Optional[str] = None, db: Session = Depends(get_db)):
    query = db.query(db_model.Node)
    if parent_id:
        query = query.filter(db_model.Node.parent_id == parent_id)
    else:
        query = query.filter(db_model.Node.parent_id == None) # Root 레벨
    return query.all()

# 2. 특정 노드 상세 조회
@app.get("/nodes/{node_id}")
def read_node(node_id: str, db: Session = Depends(get_db)):
    node = db.query(db_model.Node).filter(db_model.Node.id == node_id).first()
    if not node:
        raise HTTPException(status_code=404, detail="Node not found")
    return node

# 3. 노드 생성 (하우스, 스토리지, 아이템 공용)
@app.post("/nodes")
def create_node(node_data: dict, db: Session = Depends(get_db)):
    new_node = db_model.Node(**node_data)
    db.add(new_node)
    db.commit()
    db.refresh(new_node)
    return new_node

# 4. 노드 삭제
@app.delete("/nodes/{node_id}")
def delete_node(node_id: str, db: Session = Depends(get_db)):
    node = db.query(db_model.Node).filter(db_model.Node.id == node_id).first()
    if node:
        db.delete(node)
        db.commit()
    return {"message": "Deleted successfully"}